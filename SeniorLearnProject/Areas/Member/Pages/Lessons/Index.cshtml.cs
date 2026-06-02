using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeniorLearnProject.Areas.Member.Pages.Lessons;

[Authorize(Roles = "Professional")]
public class IndexModel : PageModel
{
    private readonly SeniorLearnContext _context;

    public IndexModel(SeniorLearnContext context)
    {
        _context = context;
    }

    public IList<Lesson> Lessons { get; set; } = new List<Lesson>();

    public async Task OnGetAsync()
    {
        Lessons = await _context.Lessons
            .Include(l => l.DeliveryPlan)
            .OrderBy(l => l.Start)
            .ToListAsync();
    }
}
