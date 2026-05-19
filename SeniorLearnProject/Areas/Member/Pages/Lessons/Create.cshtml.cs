using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeniorLearnProject.Areas.Member.Pages.Lessons
{
    [Authorize(Roles = "ProfessionalMember")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public LessonInputModel Lesson { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add logic to save the lesson to the database

            return RedirectToPage("/Lessons/Index");
        }

        public class LessonInputModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}