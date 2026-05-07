using Microsoft.AspNetCore.Identity;
namespace SeniorLearnProject.Models;

public class User : IdentityUser
{
    public Member? Member { get; set; }
    public int? MemberId { get; set; }
    List<UserRole> Roles { get; set; }
}
