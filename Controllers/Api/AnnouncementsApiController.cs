using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HawassaUnifiedCampusEventManagementSystem.Data;
using HawassaUnifiedCampusEventManagementSystem.Models;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers.Api
{
    [ApiController]
    [Route("api/announcements")]
    [Produces("application/json")]
    public class AnnouncementsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AnnouncementsApiController> _logger;

        public AnnouncementsApiController(ApplicationDbContext db, ILogger<AnnouncementsApiController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // =====================================================================
        // GET /api/announcements - Active Announcements Feed
        // =====================================================================
        [HttpGet]
        public async Task<IActionResult> GetAnnouncements(
            [FromQuery] string? priority = null,
            [FromQuery] string? audience = null,
            [FromQuery] int limit = 10)
        {
            try
            {
                var now = DateTime.UtcNow;

                var query = _db.announcements
                    .Include(a => a.author)
                    .Include(a => a.department)
                    .Where(a => a.status == "PUBLISHED" && (a.expires_at == null || a.expires_at > now))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(priority))
                {
                    query = query.Where(a => a.priority == priority.ToUpper());
                }

                var list = await query
                    .OrderByDescending(a => a.published_at ?? a.created_at)
                    .Take(limit)
                    .Select(a => new
                    {
                        id = a.id,
                        title = a.title,
                        slug = a.slug,
                        summary = a.summary,
                        content = a.content,
                        type = a.announcement_type,
                        priority = a.priority,
                        author = a.author != null ? $"{a.author.first_name} {a.author.last_name}".Trim() : "University Administration",
                        department = a.department != null ? a.department.name : "Campus Directorate",
                        publishedAt = a.published_at ?? a.created_at,
                        expiresAt = a.expires_at
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = list.Count,
                    data = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error retrieving announcements");
                return StatusCode(500, new { success = false, message = "Could not retrieve announcements." });
            }
        }
    }
}
