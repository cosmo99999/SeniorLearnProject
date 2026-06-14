using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Areas.Member.Pages.Lessons;

[Authorize(Roles = "Professional")]
public class DeleteModel : PageModel
{
    private readonly SeniorLearnContext _context;

    public DeleteModel(SeniorLearnContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Lesson Lesson { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);

        if (Lesson == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var dbLesson = await _context.Lessons.FindAsync(id);

        if (dbLesson != null)
        {
            _context.Lessons.Remove(dbLesson);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}
