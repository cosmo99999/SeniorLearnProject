using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Data;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin", Policy = "ActiveRolePolicy")]
public class BaseController : Controller
{
    private readonly SeniorLearnContext _context;
    public BaseController(SeniorLearnContext context)
    {
        _context = context;
    }
}

