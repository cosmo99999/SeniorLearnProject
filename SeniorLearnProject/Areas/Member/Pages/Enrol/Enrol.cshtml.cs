using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorLearnProject.Data;


namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

[Authorize(Policy = "ActiveRolePolicy")]

public class EnrolModel : PageModel
{
    private readonly SeniorLearnContext _context;
    private readonly UserManager<Models.Identity.User> _userManager;


    //public CreateModel(SeniorLearnContext context, UserManager<Models.Identity.User> userManager)
    //{
    //    _context = context;
    //    _userManager = userManager;
    //}
    public IActionResult OnGet()
    {
        return Page();
    }
}
