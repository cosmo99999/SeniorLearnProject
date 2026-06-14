using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

[Authorize(Roles = "Standard,Professional,Honorary", Policy = "ActiveRolePolicy")]

public class CancelModel : PageModel
{
    private readonly SeniorLearnContext _context;

    public CancelModel(SeniorLearnContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Enrolment Enrolment { get; set; }
    public string StartDate { get; set; } = "";
    public string FinishDate { get; set; } = "";


    public async Task<IActionResult> OnGetAsync(int id)
    {

        Enrolment = await _context.Enrolments
            .Include(e => e.Lesson)
            .ThenInclude(l => l.DeliveryPlan)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (Enrolment?.Lesson?.DeliveryPlan == null) return NotFound();

        var deliveryPlanId = Enrolment?.Lesson?.DeliveryPlan.Id;
        if (deliveryPlanId == null) return NotFound();

        StartDate = _context.Lessons
                .Where(l => l.DeliveryPlan != null &&
                l.DeliveryPlan.Id == deliveryPlanId.Value)
                .OrderBy(l => l.Start)
                .Select(l => l.Start)
                .FirstOrDefault()
                .ToString("MMM dd, yyyy");

        FinishDate = _context.Lessons
                .Where(l => l.DeliveryPlan != null &&
                l.DeliveryPlan.Id == deliveryPlanId.Value)
                .OrderByDescending(l => l.End)
                .Select(l => l.End)
                .FirstOrDefault()
                .ToString("MMM dd, yyyy");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {

        Enrolment = await _context.Enrolments.FirstOrDefaultAsync(e => e.Id == id);

        //if (dbEnrolment != null)
        //{
        //    _context.Enrolments.Remove(dbEnrolment);
        //    await _context.SaveChangesAsync();
        //}

        var result = await CancelEnrolmentAsync(new List<int> { id });
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return Page();
        }

        return RedirectToPage("./Index");
    }

    private async Task<(bool Success, string ErrorMessage)> CancelEnrolmentAsync(List<int> enrolmentIds)
    {
        var enrolments = await _context.Enrolments
            .Where(e => enrolmentIds.Contains(e.Id))
            .ToListAsync();
        if (!enrolments.Any()) return (false, "Enrolment not found.");
        try
        {
            _context.Enrolments.RemoveRange(enrolments);
            await _context.SaveChangesAsync();
            return (true, null);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return (false, "An error occurred while cancelling the enrolment.");
        }
    }
}