using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly SeniorLearnContext _context;
        private readonly SchedulerService _schedulerService;
        public HomeController(SeniorLearnContext context, SchedulerService schedulerService)
        {
            _context = context;
            _schedulerService = schedulerService;
        }

        public IActionResult Index()
        {
            return View();
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
