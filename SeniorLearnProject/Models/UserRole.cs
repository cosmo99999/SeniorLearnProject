using Microsoft.AspNetCore.Identity;

namespace SeniorLearnProject.Models;


internal class UserRole : IdentityUserRole<string>
{
    User User { get; set; }
    Role Role { get; set; }
    bool IsActive { get; set; }
}
