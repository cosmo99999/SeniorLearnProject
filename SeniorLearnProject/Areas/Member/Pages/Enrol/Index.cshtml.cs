using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

public class IndexModel : PageModel
{
    private readonly SeniorLearnContext _context;


    public IndexModel(SeniorLearnContext context)
    {
        _context = context;
    }

    public IList<Lesson> Lessons { get; set; } = new List<Lesson>();
    public IList<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public async Task OnGetAsync()
    {
        Lessons = await _context.Lessons
            .Include(l => l.DeliveryPlan)
            .OrderBy(l => l.Start)
            .ToListAsync();
        Enrolments = await _context.Enrolments
            .Include(e => e.Lesson)
            //.Where(e => e.Member == User.Identity)
            .ToListAsync();
    }
}
