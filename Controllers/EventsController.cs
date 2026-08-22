using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using HawassaUnifiedCampusEventManagementSystem.Data;
using HawassaUnifiedCampusEventManagementSystem.Models;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<EventsController> _logger;

        public EventsController(ApplicationDbContext db, ILogger<EventsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        private ulong? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdStr) && ulong.TryParse(userIdStr, out ulong parsedId))
            {
                return parsedId;
            }
            return null;
        }

        private bool IsAdminOrSuperAdmin()
        {
            return User.IsInRole("Admin") || User.IsInRole("SuperAdmin") ||
                   User.IsInRole("ADMIN") || User.IsInRole("SUPERADMIN");
        }

        // Map EF entity to view model
        private Event ToViewModel(_event e, ulong? currentUserId = null)
        {
            if (e == null) return new Event();

            var isRegistered = false;
            if (currentUserId.HasValue && e.registrations != null && e.registrations.Any())
            {
                isRegistered = e.registrations.Any(r => r.user_id == currentUserId.Value && r.status == "REGISTERED");
            }

            var vm = new Event
            {
                Id = e.id,
                Title = e.title,
                Category = e.category?.name,
                Capacity = e.capacity.HasValue ? (int?)e.capacity.Value : null,
                Description = e.description ?? string.Empty,
                EventDate = e.start_at,
                Venue = e.venue?.name,
                StartTime = e.start_at.TimeOfDay,
                EndTime = e.end_at != default && e.end_at != e.start_at ? e.end_at.TimeOfDay : (TimeSpan?)null,
                OrganizerId = e.organizer_id,
                Organizer = e.organizer != null ? ($"{e.organizer.first_name} {e.organizer.last_name}".Trim()) : null,
                OrganizerEmail = e.organizer?.email,
                ContactPhone = e.organizer?.phone,
                IsPublished = e.is_public ?? false,
                ApprovalStatus = e.approval_status ?? "APPROVED",
                Status = e.status ?? "PUBLISHED",
                IsUserRegistered = isRegistered,
                RegisteredCount = e.registrations != null ? e.registrations.Count(r => r.status == "REGISTERED") : 0,
                ShortDescription = e.short_description,
                ImageUrl = e.image_url,
                Slug = e.slug,
                CreatedAt = e.created_at
            };

            return vm;
        }

        // =========================================================
        // GET: /Events
        // =========================================================
        public async Task<IActionResult> Index()
        {
            var currentUserId = GetCurrentUserId();
            var isAdmin = IsAdminOrSuperAdmin();

            var query = _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .Include(x => x.registrations)
                .AsQueryable();

            // Normal users and guests see published & approved events. Admins see all events.
            if (!isAdmin)
            {
                if (currentUserId.HasValue)
                {
                    query = query.Where(x => (x.is_public == true && x.approval_status == "APPROVED") || x.organizer_id == currentUserId.Value);
                }
                else
                {
                    query = query.Where(x => x.is_public == true && x.approval_status == "APPROVED");
                }
            }

            var items = await query.OrderByDescending(x => x.start_at).ToListAsync();
            var vm = items.Select(e => ToViewModel(e, currentUserId));
            return View(vm);
        }

        // =========================================================
        // GET: /Events/Details/5
        // =========================================================
        public async Task<IActionResult> Details(ulong? id)
        {
            if (id == null || id == 0) return NotFound();

            var currentUserId = GetCurrentUserId();

            var e = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .Include(x => x.registrations)
                .FirstOrDefaultAsync(x => x.id == id.Value);

            if (e == null) return NotFound();

            return View(ToViewModel(e, currentUserId));
        }

        // =========================================================
        // GET: /Events/Create
        // Allowed Roles: Faculty, Staff, Organization, Club, Admin, SuperAdmin
        // =========================================================
        [Authorize(Roles = "Faculty,Staff,Organization,Club,Admin,SuperAdmin,FACULTY,STAFF,ORGANIZATION,ADMIN,SUPERADMIN")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.event_categories.Where(c => c.is_active == true).ToListAsync();
            ViewBag.Venues = await _db.venues.Where(v => v.status == "AVAILABLE").ToListAsync();
            return View(new Event { EventDate = DateTime.Today.AddDays(3), StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(11) });
        }

        // =========================================================
        // POST: /Events/Create
        // =========================================================
        [Authorize(Roles = "Faculty,Staff,Organization,Club,Admin,SuperAdmin,FACULTY,STAFF,ORGANIZATION,ADMIN,SUPERADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.event_categories.Where(c => c.is_active == true).ToListAsync();
                ViewBag.Venues = await _db.venues.Where(v => v.status == "AVAILABLE").ToListAsync();
                return View(model);
            }

            var organizerId = GetCurrentUserId();
            if (!organizerId.HasValue)
            {
                var firstUser = await _db.users.FirstOrDefaultAsync();
                organizerId = firstUser?.id ?? 1;
            }

            var isAdmin = IsAdminOrSuperAdmin();

            // Calculate start and end times satisfying MySQL chk_events_dates (end_at > start_at)
            var startAt = model.EventDate.Date + model.StartTime;
            DateTime endAt;
            if (model.EndTime.HasValue && model.EndTime.Value > model.StartTime)
            {
                endAt = model.EventDate.Date + model.EndTime.Value;
            }
            else
            {
                endAt = startAt.AddHours(2);
            }

            // Resolve Category ID dynamically
            ulong categoryId = 1;
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                var matchingCat = await _db.event_categories.FirstOrDefaultAsync(c => c.name.ToLower() == model.Category.ToLower() || c.slug.ToLower() == model.Category.ToLower());
                if (matchingCat != null)
                {
                    categoryId = matchingCat.id;
                }
                else
                {
                    var firstCat = await _db.event_categories.FirstOrDefaultAsync();
                    if (firstCat != null) categoryId = firstCat.id;
                }
            }
            else
            {
                var firstCat = await _db.event_categories.FirstOrDefaultAsync();
                if (firstCat != null) categoryId = firstCat.id;
            }

            // Resolve Venue ID if provided
            ulong? venueId = null;
            if (!string.IsNullOrWhiteSpace(model.Venue))
            {
                var matchingVenue = await _db.venues.FirstOrDefaultAsync(v => v.name.ToLower().Contains(model.Venue.ToLower()));
                if (matchingVenue != null) venueId = matchingVenue.id;
            }

            // Generate unique slug
            var baseSlug = !string.IsNullOrWhiteSpace(model.Slug)
                ? model.Slug.Trim().ToLower().Replace(' ', '-')
                : (!string.IsNullOrWhiteSpace(model.Title) ? model.Title.Trim().ToLower().Replace(' ', '-') : "campus-event");
            var uniqueSlug = $"{baseSlug}-{Guid.NewGuid().ToString("N")[..6]}";

            // Lifecycle status: Admin/SuperAdmin are auto-approved; Faculty/Staff/Clubs go to PENDING approval
            string approvalStatus = isAdmin ? "APPROVED" : "PENDING";
            string eventStatus = isAdmin ? "PUBLISHED" : "DRAFT";
            bool isPublic = isAdmin ? model.IsPublished : false;

            var entity = new _event
            {
                title = model.Title ?? "Campus Event",
                slug = uniqueSlug,
                description = model.Description,
                short_description = model.ShortDescription,
                start_at = startAt,
                end_at = endAt,
                capacity = model.Capacity.HasValue ? (uint?)model.Capacity.Value : null,
                is_public = isPublic,
                image_url = model.ImageUrl,
                organizer_id = organizerId.Value,
                category_id = categoryId,
                venue_id = venueId,
                event_mode = "IN_PERSON",
                status = eventStatus,
                approval_status = approvalStatus,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            _db.events.Add(entity);
            await _db.SaveChangesAsync();

            if (isAdmin)
            {
                TempData["SuccessMessage"] = "Event created and published successfully!";
            }
            else
            {
                TempData["SuccessMessage"] = "Event submitted successfully! It is now pending administrative approval before being published on campus.";
            }

            return RedirectToAction(nameof(Details), new { id = (long)entity.id });
        }

        // =========================================================
        // GET: /Events/Edit/5
        // Allowed: Event Organizer (Owner) OR Admin / SuperAdmin
        // =========================================================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(ulong? id)
        {
            if (id == null || id == 0) return NotFound();

            var currentUserId = GetCurrentUserId();
            var isAdmin = IsAdminOrSuperAdmin();

            var e = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .FirstOrDefaultAsync(x => x.id == id.Value);

            if (e == null) return NotFound();

            // Enforce ownership or admin privilege
            if (e.organizer_id != currentUserId && !isAdmin)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this event.";
                return Forbid();
            }

            ViewBag.Categories = await _db.event_categories.Where(c => c.is_active == true).ToListAsync();
            ViewBag.Venues = await _db.venues.Where(v => v.status == "AVAILABLE").ToListAsync();

            return View(ToViewModel(e, currentUserId));
        }

        // =========================================================
        // POST: /Events/Edit/5
        // =========================================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ulong id, Event model)
        {
            if (id != model.Id) return BadRequest();

            var currentUserId = GetCurrentUserId();
            var isAdmin = IsAdminOrSuperAdmin();

            var e = await _db.events.FindAsync(id);
            if (e == null) return NotFound();

            // Enforce ownership or admin privilege
            if (e.organizer_id != currentUserId && !isAdmin)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.event_categories.Where(c => c.is_active == true).ToListAsync();
                ViewBag.Venues = await _db.venues.Where(v => v.status == "AVAILABLE").ToListAsync();
                return View(model);
            }

            var startAt = model.EventDate.Date + model.StartTime;
            var endAt = (model.EndTime.HasValue && model.EndTime.Value > model.StartTime)
                ? model.EventDate.Date + model.EndTime.Value
                : startAt.AddHours(2);

            // update fields
            e.title = model.Title ?? e.title;
            e.description = model.Description;
            e.short_description = model.ShortDescription;
            e.start_at = startAt;
            e.end_at = endAt;
            e.capacity = model.Capacity.HasValue ? (uint?)model.Capacity.Value : e.capacity;
            if (isAdmin)
            {
                e.is_public = model.IsPublished;
            }
            e.image_url = model.ImageUrl;
            e.updated_at = DateTime.UtcNow;

            _db.events.Update(e);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // =========================================================
        // POST: /Events/Delete/5
        // Allowed: Event Organizer (Owner) OR Admin / SuperAdmin
        // =========================================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ulong id)
        {
            var currentUserId = GetCurrentUserId();
            var isAdmin = IsAdminOrSuperAdmin();

            var e = await _db.events.FindAsync(id);
            if (e == null) return NotFound();

            // Enforce ownership or admin privilege
            if (e.organizer_id != currentUserId && !isAdmin)
            {
                return Forbid();
            }

            _db.events.Remove(e);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // POST: /Events/RegisterEvent/5 (Student / Attendee Registration)
        // =========================================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEvent(ulong id)
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Events", new { id }) });
            }

            var e = await _db.events
                .Include(ev => ev.registrations)
                .FirstOrDefaultAsync(ev => ev.id == id);

            if (e == null) return NotFound();

            // Check if already registered
            var existingReg = await _db.registrations
                .FirstOrDefaultAsync(r => r.event_id == id && r.user_id == currentUserId.Value);

            if (existingReg != null)
            {
                if (existingReg.status == "REGISTERED")
                {
                    TempData["InfoMessage"] = "You are already registered for this event!";
                    return RedirectToAction(nameof(Details), new { id });
                }

                // Re-activate if previously cancelled
                existingReg.status = "REGISTERED";
                existingReg.registered_at = DateTime.UtcNow;
                existingReg.cancelled_at = null;
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your event registration has been reactivated successfully!";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Check capacity
            var activeCount = e.registrations.Count(r => r.status == "REGISTERED");
            string regStatus = (e.capacity.HasValue && activeCount >= e.capacity.Value) ? "WAITLISTED" : "REGISTERED";

            var regCode = $"REG-HU-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            var qrToken = Guid.NewGuid().ToString("N");

            var newReg = new Registration
            {
                event_id = id,
                user_id = currentUserId.Value,
                registration_code = regCode,
                qr_token = qrToken,
                status = regStatus,
                registered_at = DateTime.UtcNow
            };

            _db.registrations.Add(newReg);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = regStatus == "REGISTERED"
                ? $"Registration confirmed! Your ticket code is {regCode}."
                : "The event is currently at full capacity. You have been added to the waitlist.";

            return RedirectToAction(nameof(Details), new { id });
        }

        // =========================================================
        // POST: /Events/CancelRegistration/5
        // =========================================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRegistration(ulong id)
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue) return Unauthorized();

            var reg = await _db.registrations
                .FirstOrDefaultAsync(r => r.event_id == id && r.user_id == currentUserId.Value && r.status == "REGISTERED");

            if (reg != null)
            {
                reg.status = "CANCELLED";
                reg.cancelled_at = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your registration has been cancelled.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // =========================================================
        // GET: /Events/MyEvents
        // =========================================================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyEvents()
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return View(Enumerable.Empty<Event>());
            }

            var userEvents = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .Include(x => x.registrations)
                .Where(x => x.organizer_id == currentUserId.Value)
                .OrderByDescending(x => x.start_at)
                .ToListAsync();

            var vm = userEvents.Select(e => ToViewModel(e, currentUserId)).ToList();
            return View(vm);
        }

        // =========================================================
        // GET: /Events/Search?q=term
        // =========================================================
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return View(new List<Event>());

            var currentUserId = GetCurrentUserId();

            var items = await _db.events
                .Where(x => x.title.Contains(q) && (x.is_public == true && x.approval_status == "APPROVED"))
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .Include(x => x.registrations)
                .OrderByDescending(x => x.start_at)
                .ToListAsync();

            var vm = items.Select(e => ToViewModel(e, currentUserId));
            return View(vm);
        }

        // =========================================================
        // GET: /Events/Categories
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            try
            {
                var categories = await _db.event_categories
                    .Where(c => c.is_active == true)
                    .OrderBy(c => c.name)
                    .Select(c => new EventCategorySummaryViewModel
                    {
                        Id = c.id,
                        Name = c.name,
                        Slug = c.slug,
                        Description = c.description,
                        Icon = c.icon,
                        IsActive = c.is_active ?? true,
                        EventCount = c._events.Count(e => e.is_public == true && e.approval_status == "APPROVED" && (e.status == "PUBLISHED" || e.status == "UPCOMING"))
                    })
                    .ToListAsync();

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching event categories summary from database.");
                return View(new List<EventCategorySummaryViewModel>());
            }
        }
    }
}