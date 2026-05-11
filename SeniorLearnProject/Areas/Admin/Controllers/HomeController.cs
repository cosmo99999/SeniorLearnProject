using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Areas.Admin.Models;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Areas.Admin.Controllers;

public class HomeController : BaseController
{
    private readonly AdminService _aService;
    public HomeController(SeniorLearnContext context, AdminService aService)
        :base(context)
    {
        _aService = aService;
    }
    public async Task<IActionResult> Index()
    {
        List<User> usersWithoutRegistration = await _aService.GetUsersWithNoMember();
        return View(usersWithoutRegistration);
    }
    public async Task<IActionResult> Details()
    {
        return View();
    }

    // [HttpPost]
    // public async Task<bool> Index(RegisterMember m)
    // {
    //     if (ModelState.IsValid)
    //     {
            
    //     }
    // }

}
