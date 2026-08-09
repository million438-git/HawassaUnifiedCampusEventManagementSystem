using HawassaUnifiedCampusEventManagementSystem.Data;
using HawassaUnifiedCampusEventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.events
                .AsNoTracking()
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(ulong? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(_event eventItem)
        {
            if (!ModelState.IsValid)
                return View(eventItem);

            _context.events.Add(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(ulong? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ulong id, _event eventItem)
        {
            if (id != eventItem.id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(eventItem);

            try
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventItem.id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(ulong? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ulong id)
        {
            var eventItem = await _context.events.FindAsync(id);

            if (eventItem != null)
            {
                _context.events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(ulong id)
        {
            return _context.events.Any(e => e.id == id);
        }
    }
}