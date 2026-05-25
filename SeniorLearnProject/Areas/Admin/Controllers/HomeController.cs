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
        userModels = await _uService.ConvertUserToAdminUserModel(usersWithoutRegistration);
        return View(userModels);
    }
    
    public async Task<IActionResult> Details(string id)
    {
        var u = await _uService.GetUserById(id);
        var uModel = await _uService.ConvertUserToAdminUserModel(u);
        return View(uModel);
    }
    [HttpPost]
    public async Task<IActionResult> Details(UserModel model)
    {
        if (!model.MemberId.HasValue)
        {
            _uService.CreateMember(model);
        }
        await _uService.SaveUserModelChanges(model);
        return View(model);
    }
    public async Task<IActionResult> Search()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Search(string name)
    {
        var users = await _uService.FindUsersByName(name);
        List<UserModel> uModels = await _uService.ConvertUserToAdminUserModel(users);
        return View(uModels);
    }
    // [HttpPost]
    // public async Task<bool> Index(RegisterMember m)
    // {
    //     if (ModelState.IsValid)
    //     {
            
    //     }
    // }

}
