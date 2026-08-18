using HawassaUnifiedCampusEventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HawassaUnifiedCampusEventManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // =====================================================
        // HOME PAGE
        // URL: /
        // URL: /Home
        // =====================================================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // =====================================================
        // ABOUT PAGE
        // URL: /Home/About
        // =====================================================

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Title"] = "About HUCEMS";

            return View();
        }


        // =====================================================
        // PRIVACY PAGE
        // URL: /Home/Privacy
        // =====================================================

        [HttpGet]
        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy Policy";

            return View();
        }


        // =====================================================
        // ERROR PAGE
        // =====================================================

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true
        )]
        public IActionResult Error()
        {
            var requestId =
                Activity.Current?.Id
                ?? HttpContext.TraceIdentifier;

            return View(
                new ErrorViewModel
                {
                    RequestId = requestId
                }
            );
        }
    }
}