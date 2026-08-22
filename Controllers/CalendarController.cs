using Microsoft.AspNetCore.Mvc;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class CalendarController : Controller
    {
        // =====================================================
        // CALENDAR PAGE
        // URL: /Calendar
        // URL: /Calendar/Index
        // =====================================================

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Campus Calendar";
            return View();
        }
    }
}
