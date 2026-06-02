using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using System.ComponentModel.DataAnnotations;

namespace SeniorLearnProject.Areas.Member.Pages.Lessons;

[Authorize(Roles = "Professional")]
public class CreateModel : PageModel
{
    private readonly SeniorLearnContext _context;
    private readonly UserManager<Models.Identity.User> _userManager;

    public CreateModel(SeniorLearnContext context, UserManager<Models.Identity.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Compatibility shims for Edit-and-Continue / incremental build when renames happened.
    // Kept minimal to satisfy any remaining references during a debug session restart.
    public class LessonInputModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } ="";

        [Required(ErrorMessage = "Start date and time is required")]
        [Display(Name = "Start Time")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "End date and time is required")]
        [Display(Name = "End Time")]
        public DateTime End { get; set; }
    }

    public class CourseInputModel
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

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute")]
        public int DurationMinutes { get; set; } = 60;

        // Weekday flags
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }

    [BindProperty]
    public UnifiedInputModel Input { get; set; } = default!;

    public IActionResult OnGet()
    {
        // Default start time to next hour, end time to next hour + 1
        var now = DateTime.Now;
        var defaultStart = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
        
        Input = new UnifiedInputModel
        {
            StartDate = defaultStart.Date,
            StartTime = defaultStart.TimeOfDay,
            EndTime = defaultStart.AddHours(1).TimeOfDay,
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Compose Start and End DateTimes for single lesson
        var lessonStart = Input.StartDate.Date + Input.StartTime;
        var lessonEnd = Input.StartDate.Date + Input.EndTime;

        if (lessonStart >= lessonEnd)
        {
            ModelState.AddModelError(string.Empty, "End time must be after start time.");
            return Page();
        }

        if (lessonStart < DateTime.Now)
        {
            ModelState.AddModelError(string.Empty, "Start time cannot be in the past.");
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Unable to load your user.");
            return Page();
        }

        int? memberId = user.MemberId;
        if (!memberId.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Unable to determine your member profile.");
            return Page();
        }

        var member = await _context.Members.FindAsync(memberId.Value);
        if (member == null)
        {
            ModelState.AddModelError(string.Empty, "Member profile not found.");
            return Page();
        }

        int mid = memberId.Value;

        if (!Input.IsCourse)
        {
            // Single lesson path
            var overlapping = _context.Lessons
                .Where(l => l.DeliveryPlan != null
                            && l.DeliveryPlan.MemberId != null
                            && l.DeliveryPlan.MemberId == mid
                            && l.Start < lessonEnd
                            && l.End > lessonStart)
                .Any();

            if (overlapping)
            {
                ModelState.AddModelError(string.Empty, "You already have a lesson scheduled in that time range.");
                return Page();
            }

            var newLesson = new Lesson(Input.Title, lessonStart, lessonEnd);
            var dp = new DeliveryPlan(new List<Lesson> { newLesson }, isCourse: false)
            {
                MemberId = mid
            };
            newLesson.DeliveryPlan = dp;
            _context.DeliveryPlans.Add(dp);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        // Course path: requires CourseEndDate and weekdays
        if (!Input.CourseEndDate.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Course end date is required for a course.");
            return Page();
        }

        if (Input.CourseEndDate.Value < Input.StartDate)
        {
            ModelState.AddModelError(string.Empty, "Course end date must be on or after the start date.");
            return Page();
        }

        //selectDays : Bool[7] -> List<DayOfWeek>

        var selectedDays = new List<DayOfWeek>();
        if (Input.Monday) selectedDays.Add(DayOfWeek.Monday);
        if (Input.Tuesday) selectedDays.Add(DayOfWeek.Tuesday);
        if (Input.Wednesday) selectedDays.Add(DayOfWeek.Wednesday);
        if (Input.Thursday) selectedDays.Add(DayOfWeek.Thursday);
        if (Input.Friday) selectedDays.Add(DayOfWeek.Friday);
        if (Input.Saturday) selectedDays.Add(DayOfWeek.Saturday);
        if (Input.Sunday) selectedDays.Add(DayOfWeek.Sunday);

        if (selectedDays.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Please select at least one weekday for the course.");
            return Page();
        }

        var lessons = new List<Lesson>();
        var cursor = Input.StartDate.Date;
        var endDate = Input.CourseEndDate.Value.Date;

        while (cursor <= endDate)
        {
            if (selectedDays.Contains(cursor.DayOfWeek))
            {
                var s = cursor.Date + Input.StartTime;
                var e = cursor.Date + Input.EndTime;
                if (s >= Input.StartDate && e <= Input.CourseEndDate.Value.AddDays(1).Date.AddTicks(-1))
                {
                    lessons.Add(new Lesson(Input.Title, s, e));
                }
            }
            cursor = cursor.AddDays(1);
        }

        if (lessons.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "No lessons could be created with the provided dates, times, and weekdays.");
            return Page();
        }

        // Check overlaps
        foreach (var l in lessons)
        {
            var overlapping = _context.Lessons
                .Where(x => x.DeliveryPlan != null
                            && x.DeliveryPlan.MemberId != null
                            && x.DeliveryPlan.MemberId == mid
                            && x.Start < l.End
                            && x.End > l.Start)
                .Any();

            if (overlapping)
            {
                ModelState.AddModelError(string.Empty, "One or more generated lessons overlap with existing scheduled lessons.");
                return Page();
            }
        }

        var dpCourse = new DeliveryPlan(lessons, isCourse: true) { MemberId = mid };
        foreach (var l in lessons)
        {
            l.DeliveryPlan = dpCourse;
            _context.Lessons.Add(l);
        }
        _context.DeliveryPlans.Add(dpCourse);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    public class UnifiedInputModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public bool IsCourse { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CourseEndDate { get; set; }

        // Weekday flags
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public int DurationMinutes => (int)(EndTime - StartTime).TotalMinutes;
    }

}


