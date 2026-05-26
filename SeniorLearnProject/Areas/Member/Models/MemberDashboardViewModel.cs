using SeniorLearnProject.Models;

namespace SeniorLearnProject.Areas.Member.Models;

public class MemberDashboardViewModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime PaidUntil { get; set; }
    public bool MembershipActive => PaidUntil >= DateTime.Now;

    public List<Enrolment> UpcomingEnrolments { get; set; } = new();
    public List<Enrolment> PastEnrolments { get; set; } = new();
}
