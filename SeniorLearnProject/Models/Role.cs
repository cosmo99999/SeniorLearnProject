using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;

enum RoleType
{
    Standard,
    Professional,
    Honorary
}
internal class Role
{

    public DateTime StartDate { get; private set; }
    public RoleType RoleType { get; private set; }

    public bool IsActive { get; private set; }

    public Role(RoleType roleType)
    {
        IsActive = true;
        StartDate = DateTime.Now;
        RoleType = roleType;
    }
    
}
