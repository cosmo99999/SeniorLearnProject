using Microsoft.AspNetCore.Mvc;
using SeniorLearnProject.Data;

namespace SeniorLearnProject.Areas.Member.Controllers;

[Area("Member")]
//[Authorize(Roles ="Member,Admin")]
public class BaseController : Controller
{
    protected readonly SeniorLearnContext _context;
    public BaseController( SeniorLearnContext seniorLearnContext)
    {
        _context = seniorLearnContext;
    }
}
