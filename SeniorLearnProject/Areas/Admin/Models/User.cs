using System;
using System.ComponentModel;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Areas.Admin.Models;

public enum UserModelRoleType
{
    Admin,
    Standard,
    Professional,
    Honorary
}
public class UserModelRole
{
    public UserModelRoleType role;
    public bool isActive;
}
public class UserModel
{
    public string Id;
    public int? MemberId;
    public string FirstName {get;set;} = default!;
    public string LastName {get;set;} = default!;
    public string Email {get;set;} = default!;
    public DateTime paidUntil = DateTime.Now;
    public List<UserModelRole> Roles = new();
    public UserModel()
    {
        Roles.Add(new UserModelRole{role = UserModelRoleType.Admin, isActive = false});
        Roles.Add(new UserModelRole{role = UserModelRoleType.Standard, isActive = false});
        Roles.Add(new UserModelRole{role = UserModelRoleType.Professional, isActive = false});
        Roles.Add(new UserModelRole{role = UserModelRoleType.Honorary, isActive = false});
    }
}
