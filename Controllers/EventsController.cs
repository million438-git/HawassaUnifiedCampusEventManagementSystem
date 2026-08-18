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

        // Map EF entity to view model
        private Event ToViewModel(_event e)
        {
            if (e == null) return new Event();

            var vm = new Event
            {
                Id = (int)e.id,
                Title = e.title,
                Category = e.category?.name,
                Capacity = e.capacity.HasValue ? (int?)e.capacity.Value : null,
                Description = e.description ?? string.Empty,
                EventDate = e.start_at,
                Venue = e.venue?.name,
                StartTime = e.start_at.TimeOfDay,
                EndTime = e.end_at != default && e.end_at != e.start_at ? e.end_at.TimeOfDay : (TimeSpan?)null,
                Organizer = e.organizer != null ? ($"{e.organizer.first_name} {e.organizer.last_name}".Trim()) : null,
                OrganizerEmail = e.organizer?.email,
                ContactPhone = e.organizer?.phone,
                IsPublished = e.is_public ?? false,
                ShortDescription = e.short_description,
                ImageUrl = e.image_url,
                Slug = e.slug,
                CreatedAt = e.created_at
            };

            return vm;
        }

        // GET: /Events
        public async Task<IActionResult> Index()
        {
            var items = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .OrderByDescending(x => x.start_at)
                .ToListAsync();

            var vm = items.Select(ToViewModel);
            return View(vm);
        }

        // GET: /Events/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var e = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .FirstOrDefaultAsync(x => x.id == (ulong)id.Value);

            if (e == null) return NotFound();

            return View(ToViewModel(e));
        }

        // GET: /Events/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Event { EventDate = DateTime.Today, StartTime = TimeSpan.FromHours(9) });
        }

        // POST: /Events/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ulong? organizerId = null;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdStr) && ulong.TryParse(userIdStr, out ulong parsedId))
            {
                organizerId = parsedId;
            }

            var entity = new _event
            {
                title = model.Title ?? string.Empty,
                slug = string.IsNullOrWhiteSpace(model.Slug) ? (model.Title ?? string.Empty).ToLower().Replace(' ', '-') : model.Slug,
                description = model.Description,
                short_description = model.ShortDescription,
                start_at = model.EventDate.Date + model.StartTime,
                end_at = model.EventDate.Date + (model.EndTime ?? model.StartTime),
                capacity = model.Capacity.HasValue ? (uint?)model.Capacity.Value : null,
                is_public = model.IsPublished,
                image_url = model.ImageUrl,
                organizer_id = organizerId ?? 1,
                category_id = 1,
                event_mode = "IN_PERSON",
                status = "PUBLISHED",
                approval_status = "APPROVED",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _db.events.Add(entity);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event created successfully!";
            return RedirectToAction(nameof(Details), new { id = (long)entity.id });
        }

        // GET: /Events/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var e = await _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer)
                .FirstOrDefaultAsync(x => x.id == (ulong)id.Value);

            if (e == null) return NotFound();

            return View(ToViewModel(e));
        }

        // POST: /Events/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Event model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return View(model);

            var e = await _db.events.FindAsync((ulong)id);
            if (e == null) return NotFound();

            // update fields
            e.title = model.Title ?? e.title;
            e.description = model.Description;
            e.short_description = model.ShortDescription;
            e.start_at = model.EventDate.Date + model.StartTime;
            e.end_at = model.EventDate.Date + (model.EndTime ?? model.StartTime);
            e.capacity = model.Capacity.HasValue ? (uint?)model.Capacity.Value : e.capacity;
            e.is_public = model.IsPublished;
            e.image_url = model.ImageUrl;
            e.updated_at = DateTime.Now;

            _db.events.Update(e);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /Events/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var e = await _db.events.FindAsync((ulong)id);
            if (e == null) return NotFound();

            _db.events.Remove(e);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Events/MyEvents
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyEvents()
        {
            ulong? currentUserId = null;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdStr) && ulong.TryParse(userIdStr, out ulong parsedId))
            {
                currentUserId = parsedId;
            }

            IQueryable<_event> query = _db.events
                .Include(x => x.category)
                .Include(x => x.venue)
                .Include(x => x.organizer);

            if (currentUserId.HasValue)
            {
                var userEvents = await query
                    .Where(x => x.organizer_id == currentUserId.Value)
                    .OrderByDescending(x => x.start_at)
                    .ToListAsync();

                if (userEvents.Any())
                {
                    return View(userEvents.Select(ToViewModel));
                }
            }

            var allItems = await query
                .OrderByDescending(x => x.start_at)
                .ToListAsync();

            var vm = allItems.Select(ToViewModel);
            return View(vm);
        }

        // GET: /Events/Search?q=term
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return View(new List<Event>());

            var items = await _db.events
                .Where(x => x.title.Contains(q))
                .Include(x => x.category)
                .Include(x => x.venue)
                .OrderByDescending(x => x.start_at)
                .ToListAsync();

            var vm = items.Select(ToViewModel);
            return View(vm);
        }
    }
}