using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HawassaUnifiedCampusEventManagementSystem.Models;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers.Api
{
    [ApiController]
    [Route("api/jobs")]
    [Produces("application/json")]
    public class JobsApiController : ControllerBase
    {
        private readonly ILogger<JobsApiController> _logger;

        public JobsApiController(ILogger<JobsApiController> logger)
        {
            _logger = logger;
        }

        // =====================================================================
        // 1. GET /api/jobs - List Jobs & Internships
        // =====================================================================
        [HttpGet]
        public IActionResult GetJobs(
            [FromQuery] string? search = null,
            [FromQuery] string? jobType = null,
            [FromQuery] string? department = null)
        {
            var jobs = GetSampleJobs();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                jobs = jobs.Where(j => j.Title.ToLower().Contains(s) || j.CompanyName.ToLower().Contains(s) || j.Description.ToLower().Contains(s)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(jobType))
            {
                jobs = jobs.Where(j => j.JobType.Equals(jobType.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(new
            {
                success = true,
                total = jobs.Count,
                data = jobs
            });
        }

        // =====================================================================
        // 2. GET /api/jobs/{id} - Get Job Details
        // =====================================================================
        [HttpGet("{id}")]
        public IActionResult GetJobById(ulong id)
        {
            var job = GetSampleJobs().FirstOrDefault(j => j.Id == id);
            if (job == null)
            {
                return NotFound(new { success = false, message = "Job posting not found." });
            }

            return Ok(new { success = true, data = job });
        }

        private static List<JobPostingViewModel> GetSampleJobs()
        {
            return new List<JobPostingViewModel>
            {
                new()
                {
                    Id = 1,
                    Title = "Junior Full-Stack Web Developer Intern",
                    Slug = "junior-full-stack-web-developer-intern",
                    CompanyName = "Hawassa Tech Hub & Innovation Center",
                    CompanyInitials = "HT",
                    CompanyColor = "#0d6efd",
                    Industry = "Software & IT",
                    JobType = "Internship",
                    WorkplaceType = "Hybrid",
                    Location = "Main Campus, Hawassa",
                    CampusLocation = "ICT Center, Building 4",
                    Description = "Work alongside senior engineers developing campus portal modules, student utilities, and web services using ASP.NET Core and modern frontend tools.",
                    ShortDescription = "Paid software engineering internship for 3rd and 4th year CS/IT/SE students.",
                    Requirements = new List<string> { "Enrolled in Computer Science, Software Engineering, or related field", "Knowledge of C#, JavaScript, HTML/CSS, SQL", "Good problem-solving and team collaboration skills" },
                    Responsibilities = new List<string> { "Develop and test web application features", "Assist in database schema optimization", "Participate in sprint standups and code reviews" },
                    Skills = new List<string> { "C#", "ASP.NET Core", "SQL", "JavaScript", "Git" },
                    SalaryDisplay = "ETB 6,500 - 8,500 / month",
                    Deadline = DateTime.Today.AddDays(14),
                    IsFeatured = true,
                    IsNew = true
                },
                new()
                {
                    Id = 2,
                    Title = "Campus Network & Cyber Defense Assistant",
                    Slug = "campus-network-cyber-defense-assistant",
                    CompanyName = "Hawassa University ICT Directorate",
                    CompanyInitials = "HU",
                    CompanyColor = "#198754",
                    Industry = "Cybersecurity & Networking",
                    JobType = "Part-Time",
                    WorkplaceType = "On-site",
                    Location = "Main Campus, Hawassa",
                    CampusLocation = "ICT Server Room",
                    Description = "Support campus network monitoring, access point maintenance, firewall rule verification, and security incident logging.",
                    ShortDescription = "Part-time on-campus role assisting the central university network operations team.",
                    Requirements = new List<string> { "Understanding of TCP/IP, DNS, routing, switching", "Familiarity with Linux and network monitoring tools" },
                    Responsibilities = new List<string> { "Monitor campus Wi-Fi infrastructure", "Assist with lab workstation security configurations" },
                    Skills = new List<string> { "Networking", "Linux", "Cybersecurity", "Troubleshooting" },
                    SalaryDisplay = "ETB 5,000 / month",
                    Deadline = DateTime.Today.AddDays(20),
                    IsFeatured = false,
                    IsNew = true
                },
                new()
                {
                    Id = 3,
                    Title = "Event Media & Digital Content Creator",
                    Slug = "event-media-digital-content-creator",
                    CompanyName = "Student Union & Campus Affairs",
                    CompanyInitials = "SU",
                    CompanyColor = "#fd7e14",
                    Industry = "Media & Communications",
                    JobType = "Part-Time",
                    WorkplaceType = "On-site",
                    Location = "All Campuses, Hawassa",
                    CampusLocation = "Student Center",
                    Description = "Capture photography, video highlights, and live stream coverage for major university conferences, festivals, and student hackathons.",
                    ShortDescription = "Create multimedia content, flyers, and video reels for campus events.",
                    Requirements = new List<string> { "Experience in photography/videography", "Proficiency with Canva, Adobe Premiere, or Photoshop" },
                    Responsibilities = new List<string> { "Cover live campus events", "Edit and publish highlight reels on university channels" },
                    Skills = new List<string> { "Videography", "Graphic Design", "Social Media", "Video Editing" },
                    SalaryDisplay = "ETB 4,500 / month + Event Passes",
                    Deadline = DateTime.Today.AddDays(10),
                    IsFeatured = true,
                    IsNew = false
                }
            };
        }
    }
}
