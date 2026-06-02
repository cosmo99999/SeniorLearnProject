using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SeniorLearnProject.Areas.Member.Pages.Lessons;

[Authorize(Roles = "Professional")]
public class EditModel : PageModel
{
    private readonly SeniorLearnContext _context;

    public EditModel(SeniorLearnContext context)
    {
        _context = context;
    }

    [BindProperty]
    public LessonInputModel Lesson { get; set; }

    public int LessonId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
        if (lesson == null)
        {
            return NotFound();
        }

        LessonId = lesson.Id;

        Lesson = new LessonInputModel
        {
            Title = lesson.Title,
            Start = lesson.Start,
            End = lesson.End
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Lesson.Start >= Lesson.End)
        {
            ModelState.AddModelError("Lesson.End", "End date and time must be after the Start date and time.");
            return Page();
        }

        var dbLesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
        if (dbLesson == null)
        {
            return NotFound();
        }

        dbLesson.Title = Lesson.Title;
        dbLesson.Start = Lesson.Start;
        dbLesson.End = Lesson.End;

        _context.Entry(dbLesson).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await LessonExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private async Task<bool> LessonExists(int id)
    {
        return await _context.Lessons.AnyAsync(e => e.Id == id);
    }

    public class LessonInputModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Start date and time is required")]
        [Display(Name = "Start Time")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "End date and time is required")]
        [Display(Name = "End Time")]
        public DateTime End { get; set; }
    }
}
