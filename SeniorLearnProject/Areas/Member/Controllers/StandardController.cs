using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Data;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Areas.Member.Controllers;

public class StandardController : BaseController
{
    private readonly SchedulerService _sService;
    public StandardController(SeniorLearnContext context, SchedulerService sServive)
        :base(context)
    {
        _sService = sServive;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details()
    {
       
        return View();
    }
}
