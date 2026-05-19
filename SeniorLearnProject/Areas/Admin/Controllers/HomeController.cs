using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Areas.Admin.Models;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models.Identity;
using SeniorLearnProject.Services;
using UserModel = SeniorLearnProject.Areas.Admin.Models.UserModel;

namespace SeniorLearnProject.Areas.Admin.Controllers;

public class HomeController : BaseController
{
    private readonly UserService _uService;
    public HomeController(SeniorLearnContext context, UserService uService)
        :base(context)
    {
        _uService = uService;
    }
    public async Task<IActionResult> Index()
    {
        List<UserModel> userModels = new();
        var usersWithoutRegistration = await _uService.GetUsersWithNoMember();
        foreach(var u in usersWithoutRegistration)
        {
            UserModel us = new();
            us.Email = u.UserName!;
            userModels.Add(us);
        }
        return View(userModels);
    }
    
    public async Task<IActionResult> Details()
    {
        return View();
    }
    public async Task<IActionResult> Search()
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
