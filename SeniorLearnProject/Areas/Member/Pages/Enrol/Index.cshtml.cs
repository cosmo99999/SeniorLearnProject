using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

public class IndexModel : PageModel
{
    private readonly SeniorLearnContext _context;
    private readonly UserManager<User> _userManager;


    public IndexModel(SeniorLearnContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<Lesson> Lessons { get; set; } = new List<Lesson>();
    public IList<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    public int getEnrolmentId(Lesson lesson)
    {
        var enrolment = _context.Enrolments.FirstOrDefault(e => e.Lesson.Id == lesson.Id && e.Member.Id == Enrolments.FirstOrDefault().Member.Id);
        return enrolment?.Id ?? 0;
    }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        Lessons = await _context.Lessons
            .Include(l => l.DeliveryPlan)
            .OrderBy(l => l.Start)
            .ToListAsync();

        // If there's no authenticated user or the user isn't linked to a Member, return empty enrolments
        if (user?.MemberId == null)
        {
            Enrolments = new List<Enrolment>();
            return;
        }

        Enrolments = await _context.Enrolments
            .Include(e => e.Lesson)
            .Include(e => e.Member)
            .Where(e => e.Member != null && e.Member.Id == user.MemberId.Value)
            .ToListAsync();        
    }
}
