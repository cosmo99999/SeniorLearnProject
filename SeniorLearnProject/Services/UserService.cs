using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Areas.Admin.Models;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using SeniorLearnProject.Models.Identity;
using System.Security.Claims;

namespace SeniorLearnProject.Services;

public class UserService
{
    private readonly SeniorLearnContext _context;

    public UserService(SeniorLearnContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserById(string id)
    {
        var u = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        if(u == null)
        {
            return null;
        }
        return u;
    }
    public async Task<UserRole> DoesUserHaveActiveRole(ClaimsPrincipal claim, string role)
    {
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == claim.Identity!.Name);
        var roleType = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);
        
        var applicableRole = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == user!.Id && u.RoleId == roleType!.Id);

        if(applicableRole == null) return null;
        if (applicableRole!.IsActive)
        {
            return applicableRole;
        }
        return null;
    }
    public async Task<List<User>> GetUsersWithNoMember()
    {
        var result = await _context.Users.FromSql(
        $"""
           SELECT * FROM AspNetUsers 
            LEFT JOIN AspNetUserRoles
            ON AspNetUsers.Id = AspNetUserRoles.UserId WHERE AspNetUserRoles.UserId IS NULL
        """
        ).ToListAsync();
        return result;
    }
    public async Task<List<User>> FindUsersByName(string name)
    {
        var result = await _context.Users
        .Where(u => u!.UserName.Contains(name)).ToListAsync();
        return result;
    }
    public async Task<List<Areas.Admin.Models.UserModel>> ConvertUserToAdminUserModel(List<User> users)
    {
        List<UserModel> result = new();
        foreach(var u in users)
        {
            UserModel m = new();
            m.Id = u.Id;
            m.Email = u!.Email;
            result.Add(m);
        }
        return result;
    }
    public async Task<Areas.Admin.Models.UserModel> ConvertUserToAdminUserModel(User user)
    {
        UserModel m = new();
        m.Id = user.Id;
        m.Email = user!.Email;

        var allRoles = await _context.Roles.OrderBy(r => r.RoleType).ToListAsync();
        var userRoles = await _context.UserRoles
            .Include(u => u.Role)
            .Where(u => u.UserId == user.Id)
            .Select(u => new {
                RoleType = (int)u.Role.RoleType,
                u.IsActive
            })
            .ToListAsync();

        m.RoleStrings = new string[allRoles.Count()];
        m.RoleBools = new bool[allRoles.Count()];

        for (int i = 0; i < allRoles.Count(); i++)
        {
            m.RoleStrings[i] = allRoles[i].Name;
        }

        for (int i = 0; i < userRoles.Count(); i++)
        {
            if (!userRoles[i].IsActive) continue;
            m.RoleBools[userRoles[i].RoleType] = true;
        }

        if (user.MemberId.HasValue)
        {
            m.MemberId = user.MemberId.Value;
        }
        return m;
    }
    public async Task CreateMember(UserModel um)
    {
        Member m = new();
        m.FirstName = um.FirstName;
        m.LastName = um.LastName;
        m.paidUntil = DateTime.Now;
        _context.Members.Add(m);
        var user = await _context.Users.Where(u => u.Id == um.Id).FirstAsync();
        m.User = user;
        await _context.SaveChangesAsync();
    }
    public async Task SaveUserModelChanges(UserModel um)
    {
        if (um == null) return;
        var user = await _context.Users.Where(u => u.Id == um.Id).FirstAsync();

        //foreach(var umRoles in um.Roles)
        //{
        //    var role = await _context.Roles.Where(r => r.Name == umRoles.role.ToString()).FirstAsync();
        //    var userInRole = await _context.UserRoles.Where(u => u.UserId == user.Id && u.RoleId == role.Name).FirstOrDefaultAsync();
        //    if(userInRole != null)
        //    {
        //        userInRole.IsActive = true;
        //    }
        //    else
        //    {
        //        UserRole ur = new();
        //        ur.RoleId = role.Id;
        //        ur.UserId = user.Id;
        //        ur.IsActive = true;
        //        _context.UserRoles.Add(ur);
        //    }
        //}

        await _context.SaveChangesAsync();
    }
}
