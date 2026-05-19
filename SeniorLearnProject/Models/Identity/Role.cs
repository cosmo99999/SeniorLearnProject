using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
