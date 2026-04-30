using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;

public enum RoleType
{
    Standard,
    Professional,
    Honorary
}
public class Role
{
    public int Id { get; set; }
    public DateTime StartDate { get; private set; }
    public RoleType RoleType { get; private set; }

    public bool IsActive { get; private set; }

    private Role(){}
    public Role(RoleType roleType)
    {
        IsActive = true;
        StartDate = DateTime.Now;
        RoleType = roleType;
    }
    
}
