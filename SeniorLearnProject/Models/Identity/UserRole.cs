using Microsoft.AspNetCore.Identity;

namespace SeniorLearnProject.Models.Identity;


public class UserRole : IdentityUserRole<string>
{
    public bool IsActive { get; set; }
    public Role Role { get; set; } = default!;
}
