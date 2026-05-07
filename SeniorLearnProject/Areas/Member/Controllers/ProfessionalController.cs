using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Data;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Areas.Member.Controllers;

public class ProfessionalController : BaseController
{

    public ProfessionalController(SeniorLearnContext context)
        :base(context)
    {

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
