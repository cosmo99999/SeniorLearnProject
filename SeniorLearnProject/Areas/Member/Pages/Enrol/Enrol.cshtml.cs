using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Areas.Member.Pages.Enrol;

[Authorize(Roles = "Standard,Professional,Honorary", Policy = "ActiveRolePolicy")]
public class EnrolModel : PageModel
{
    private readonly SeniorLearnContext _context;
    private readonly UserManager<Models.Identity.User> _userManager;

    public EnrolModel(SeniorLearnContext context, UserManager<Models.Identity.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]  // as model requier
    public int LessonId { get; set; }
    [BindProperty]
    public int MemberId { get; set; }
    public string Title { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string StartDate { get; set; } = "";
    public bool IsCourse { get; set; }
    public string FinishDate { get; set; } = "";
    public string WeekDays { get; set; } = "";


    public async Task<IActionResult> OnGetAsync(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.DeliveryPlan)
            .FirstOrDefaultAsync(x => x.Id == id); // Load the lesson with its DeliveryPlan to determine if it's a course and to access related lessons if needed.

        if (lesson == null) return NotFound(); // Handle lesson not found

        //Find the last date of the course, if it's a course. The date of the lesson.end if it is a single lesson
        string findFinishDate(Lesson l)
        {
            if (!l.DeliveryPlan.IsCourse) return lesson.End.ToString("dd MMMM");
            var courseLessons = _context.Lessons.Where(x => x.DeliveryPlan != null && x.DeliveryPlan.Id == l.DeliveryPlan.Id);
            var lastLesson = courseLessons.OrderByDescending(x => x.End).FirstOrDefault();
            return lastLesson != null ? lastLesson.End.ToString("dd MMMM") : "";
        }

        // Assuming WeekDay is a string like "Monday,Wednesday,Friday". You may need to adjust this based on your actual data model. Filter repeated weekdays.
        string findWeekDays(Lesson l)
        {
            if (!l.DeliveryPlan.IsCourse) return l.Start.ToString("dddd");
            var courseLessons = _context.Lessons.Where(x => x.DeliveryPlan != null && x.DeliveryPlan.Id == l.DeliveryPlan.Id);
            var weekDays = courseLessons.Select(x => x.Start.ToString("dddd")).Distinct();

            //Filter repeated weekdays.

            return string.Join(", ", weekDays);
        }

        LessonId = lesson.Id;
        Title = lesson.Title;
        StartTime = lesson.Start.ToString("HH : mm");
        StartDate = lesson.Start.ToString("dd MMMM");
        IsCourse = lesson.DeliveryPlan.IsCourse;
        FinishDate = findFinishDate(lesson); 
        WeekDays = findWeekDays(lesson); 

        // Pre-fill MemberId from the current user when available so the form can post it.
        var user = await _userManager.GetUserAsync(User);
        if (user != null && user.MemberId.HasValue)
        {
            MemberId = user.MemberId.Value;
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Unable to load your user.");
            return Page();
        }

        // Ensure the user has a linked MemberId
        if (!user.MemberId.HasValue)
        {
            ModelState.AddModelError(string.Empty, "User is not linked to a member record.");
            return Page();
        }

        // Validate posted MemberId against the authenticated user's MemberId to avoid spoofing.
        // If the posted MemberId is missing or doesn't match, use the server-side value from the user.
        if (MemberId == 0 || MemberId != user.MemberId.Value)
        {
            MemberId = user.MemberId.Value;
        }

        int mid = MemberId;

        // Load the selected lesson(s). If you only intend to enrol in the single LessonId bound property:
        var lesson = await _context.Lessons
            .Include(x => x.DeliveryPlan)
            .FirstOrDefaultAsync(x => x.Id == LessonId);
        if (lesson == null) return NotFound();

        // Get the Member entity to attach to Enrolment
        var member = await _context.Members.FindAsync(mid);
        if (member == null)
        {
            ModelState.AddModelError(string.Empty, "Member record not found.");
            return Page();
        }

        // Populate Enrolment table with chosen lessons
        var result = await ValidateAndEnrollLessonsAsync(new List<int> { LessonId }, member);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage); return Page(); }

        return RedirectToPage("./Index");
    }

    private async Task<(bool Success, string ErrorMessage)> ValidateAndEnrollLessonsAsync(List<int> chosenLessonIds, Models.Member member)
    {
        if (chosenLessonIds == null || chosenLessonIds.Count == 0)
            return (false, "No lessons selected.");

        var allChosenIds = new HashSet<int>(chosenLessonIds);

        var initiallySelectedLessons = await _context.Lessons
            .Where(l => chosenLessonIds.Contains(l.Id))
            .ToListAsync();

        foreach (var lesson in initiallySelectedLessons)
        {
            if (lesson.DeliveryPlan is not null)
            {
                var courseLessonIds = await _context.Lessons
                    .Where(l => l.DeliveryPlan == lesson.DeliveryPlan)
                    .Select(l => l.Id)
                    .ToListAsync();

                foreach (var id in courseLessonIds)
                    allChosenIds.Add(id);
            }
        }

        var lessonsToEnroll = await _context.Lessons
            .Where(l => allChosenIds.Contains(l.Id))
            .ToListAsync();

        var existingEnrolments = await _context.Enrolments
            .Include(e => e.Lesson)
            .Include(e => e.Member)
            .Where(e => e.Member != null && e.Member.Id == member.Id)
            .ToListAsync();

        // Helper for interval overlap: [start, end) overlaps if start < otherEnd && otherStart < end
        static bool Overlaps(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
            => aStart < bEnd && bStart < aEnd;

        // Check for overlaps with existing enrolments.
        foreach (var lesson in lessonsToEnroll)
        {
            // Adjust these property names if your Lesson entity uses different names.
            var sStart = lesson.Start;
            var sEnd = lesson.End;

            foreach (var enrol in existingEnrolments)
            {
                var eStart = enrol.Lesson.Start;
                var eEnd = enrol.Lesson.End;

                if (Overlaps(sStart, sEnd, eStart, eEnd))
                    return (false, $"Lesson {lesson.Id} conflicts with an existing enrolment (Lesson {enrol.Lesson.Id}).");
            }
        }

        // Check for overlaps among the selected lessons themselves.
        var lessonList = lessonsToEnroll.OrderBy(l => l.Start).ToList();
        for (int i = 0; i < lessonList.Count; i++)
        {
            for (int j = i + 1; j < lessonList.Count; j++)
            {
                if (Overlaps(lessonList[i].Start, lessonList[i].End, lessonList[j].Start, lessonList[j].End))
                    return (false, $"Selected lessons {lessonList[i].Id} and {lessonList[j].Id} overlap.");
            }
        }

        // Populate Enrolment table with chosen lessons (skip those already enrolled).
        var alreadyEnrolledLessonIds = existingEnrolments.Select(e => e.Lesson.Id).ToHashSet();
        var newEnrolments = lessonsToEnroll
            .Where(l => !alreadyEnrolledLessonIds.Contains(l.Id))
            .Select(l => new Enrolment
            {
                Member = member,
                Lesson = l
            })
            .ToList();

        if (newEnrolments.Count > 0)
        {
            _context.Enrolments.AddRange(newEnrolments);
            await _context.SaveChangesAsync();
        }

        return (true, null);

    }
}