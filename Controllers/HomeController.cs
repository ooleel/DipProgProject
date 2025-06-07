using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnWebApp.Models;

namespace SeniorLearnWebApp.Controllers
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
            bool isLoggedIn = HttpContext.Session.GetInt32("MemberId") != null;
            string role = HttpContext.Session.GetString("Role") ?? "";
            
            var vm = new HomeIndexViewModel
            {
                IsLoggedIn = isLoggedIn,
                Role = role
            };

            return View(vm);
        }

        public IActionResult Privacy()
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
