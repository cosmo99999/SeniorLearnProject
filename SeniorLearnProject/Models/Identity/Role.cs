using Microsoft.AspNetCore.Identity;

namespace SeniorLearnProject.Models.Identity;

public class Role : IdentityRole
{
    public enum Type
    {
        Admin,
        Standard,
        Professional,
        Honorary
    }
    public Type RoleType { get; private set; }
    public Role(Type roleType) : base(roleType.ToString())
    {
        RoleType = roleType;
        NormalizedName = roleType.ToString().ToUpper();
    }

}
