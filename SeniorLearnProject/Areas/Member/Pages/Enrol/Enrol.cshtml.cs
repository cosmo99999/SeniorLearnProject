using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

[Authorize(Roles = "Member")]

public class EnrolModel : PageModel
{
    public void OnGet()
    {
    }
}
