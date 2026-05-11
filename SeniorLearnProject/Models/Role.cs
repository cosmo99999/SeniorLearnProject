using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;

public class Role : IdentityRole
{
    public enum Type
    {
        Admin,
        Standard,
        Professional,
        Honorary
    }
    public bool IsActive { get; private set; }
    public Type RoleType { get; private set; }
    private Role(Type roleType) : base(roleType.ToString()) { }

}
