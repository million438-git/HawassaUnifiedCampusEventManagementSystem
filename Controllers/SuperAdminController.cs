using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    [Authorize(Roles = "SUPERADMIN,SuperAdmin")]
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            TempData["InfoMessage"] = "Navigating to SuperAdmin User Management module.";
            return RedirectToAction("Users", "Admin");
        }

        public IActionResult Roles()
        {
            TempData["InfoMessage"] = "Navigating to Roles & RBAC Governance.";
            return RedirectToAction("Roles", "Admin");
        }

        public IActionResult Permissions()
        {
            TempData["InfoMessage"] = "Navigating to Permissions Matrix.";
            return RedirectToAction("Roles", "Admin");
        }

        public IActionResult Events()
        {
            TempData["InfoMessage"] = "Navigating to Platform Events Management.";
            return RedirectToAction("Events", "Admin");
        }

        public IActionResult EventApprovals()
        {
            TempData["InfoMessage"] = "Navigating to Event Approvals Queue.";
            return RedirectToAction("Events", "Admin");
        }

        public IActionResult Organizations()
        {
            TempData["InfoMessage"] = "Navigating to Campus Organizations Management.";
            return RedirectToAction("Organizations", "Admin");
        }

        public IActionResult Departments()
        {
            TempData["InfoMessage"] = "Navigating to University Departments.";
            return RedirectToAction("Departments", "Admin");
        }

        public IActionResult Faculties()
        {
            TempData["InfoMessage"] = "Navigating to Faculties Management.";
            return RedirectToAction("Faculties", "Admin");
        }

        public IActionResult Venues()
        {
            TempData["InfoMessage"] = "Navigating to Campus Venues.";
            return RedirectToAction("Venues", "Admin");
        }

        public IActionResult Announcements()
        {
            TempData["InfoMessage"] = "Navigating to University Announcements.";
            return RedirectToAction("Announcements", "Admin");
        }

        public IActionResult Reports()
        {
            TempData["InfoMessage"] = "Navigating to System Reports & Content Moderation.";
            return RedirectToAction("Reports", "Admin");
        }

        public IActionResult UserSuspensions()
        {
            TempData["InfoMessage"] = "Navigating to User Suspensions & Security Clearance.";
            return RedirectToAction("Users", "Admin");
        }

        public IActionResult SystemSettings()
        {
            TempData["InfoMessage"] = "Navigating to Global System Settings.";
            return RedirectToAction("Settings", "Admin");
        }

        public IActionResult AuditLogs()
        {
            TempData["InfoMessage"] = "Navigating to Security Audit Logs Vault.";
            return RedirectToAction("AuditLogs", "Admin");
        }

        public IActionResult DatabaseManagement()
        {
            TempData["InfoMessage"] = "Navigating to Database Management & Health Telemetry.";
            return RedirectToAction("DatabaseBackup", "Admin");
        }
    }
}
