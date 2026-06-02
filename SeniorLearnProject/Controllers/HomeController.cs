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
        private readonly UserService _uServive;
        public HomeController(SeniorLearnContext context, SchedulerService schedulerService, UserService uService)
        {
            _context = context;
            _schedulerService = schedulerService;
            _uServive = uService;
        }

        public async Task<IActionResult> Index()
        {
            var u = User.Identity.Name;
            if(u == null)
            {
                return View();
            }
            else
            {
                var a = await _uServive.DoesUserHaveActiveRole(User, "Admin");
                var s = await _uServive.DoesUserHaveActiveRole(User, "Standard");
                var p = await _uServive.DoesUserHaveActiveRole(User, "Professional");
                var h = await _uServive.DoesUserHaveActiveRole(User, "Honorary");
                
                if(a != null && a == true)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
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
