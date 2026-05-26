using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Areas.Member.Models;
using SeniorLearnProject.Data;

namespace SeniorLearnProject.Areas.Member.Controllers;

public class HomeController : BaseController
{
    public HomeController(SeniorLearnContext context)
        : base(context)
    {
    }

    public async Task<IActionResult> Index()
    {
        var member = await _context.Members
            .Include(m => m.User)
            .Include(m => m.Enrolments)
                .ThenInclude(e => e.Lesson)
            .FirstOrDefaultAsync(m => m.User.UserName == User.Identity!.Name);

        if (member == null) return View("Error");

        var now = DateTime.Now;

        var viewModel = new MemberDashboardViewModel
        {
            FirstName          = member.FirstName,
            LastName           = member.LastName,
            Email              = member.User.Email ?? "",
            PaidUntil          = member.paidUntil,
            UpcomingEnrolments = member.Enrolments
                .Where(e => e.Lesson.End > now)
                .OrderBy(e => e.Lesson.Start)
                .ToList(),
            PastEnrolments     = member.Enrolments
                .Where(e => e.Lesson.End <= now)
                .OrderByDescending(e => e.Lesson.Start)
                .ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var enrolment = await _context.Enrolments
            .Include(e => e.Lesson)
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enrolment == null) return NotFound();

        return View(enrolment);
    }
}
