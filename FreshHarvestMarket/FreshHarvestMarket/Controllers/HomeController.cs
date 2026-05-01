/*
 * HomeController.cs
 * FreshHarvestMarket
 *
 * This controller handles general navigation & static pages
 * for the application.
 *
 * It is responsible for:
 * - Displaying the home (landing) page
 * - Providing access to informational pages such as About & Privacy
 * - Serving the login page entry point
 * - Handling application-level error display
 *
 * This controller does not directly manage business logic or data,
 * but instead routes users to the appropriate views.
 */

using System.Diagnostics;
using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the login page
        /// </summary>
        /// <returns>Login view</returns>
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
