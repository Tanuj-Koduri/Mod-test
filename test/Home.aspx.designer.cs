using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ModernPimsApp.Controllers
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
            _logger.LogInformation("Home page visited");
            return View();
        }

        public IActionResult Logout()
        {
            // Implement logout logic
            return RedirectToAction("Index", "Login");
        }

        public IActionResult RegisterComplaint()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterComplaint(ComplaintViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the complaint
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}