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
        var usersRoles = await _context.UserRoles.Where(u => u.UserId == user.Id).ToListAsync();
        
        foreach(var role in usersRoles)
        {
            if (role.IsActive)
            {
                var roleDetails = await _context.Roles.Where(r => r.Id == role.RoleId).FirstOrDefaultAsync();
                for(int i = 0; i < m.Roles.Count(); i++)
                {
                    var mRole = m.Roles[i];
                    if(mRole.role.ToString() == roleDetails.Name)
                    {
                        mRole.isActive = true;
                    }
                }
            }
        }
        if(user.MemberId.HasValue)
        {
            m.MemberId = user.MemberId.Value;
        }
        return m;
    }
    public async void CreateMember(UserModel um)
    {
        Member m = new();
        m.FirstName = um.FirstName;
        m.paidUntil = DateTime.Now;
        _context.Members.Add(m);
        await _context.SaveChangesAsync();
        um.MemberId = m.Id;
    }
    public async Task SaveUserModelChanges(UserModel um)
    {
        var user = await _context.Users.Where(u => u.Id == um.Id).FirstAsync();

        foreach(var umRoles in um.Roles)
        {
            var role = await _context.Roles.Where(r => r.Name == umRoles.role.ToString()).FirstAsync();
            var userInRole = await _context.UserRoles.Where(u => u.UserId == user.Id && u.RoleId == role.Name).FirstOrDefaultAsync();
            if(userInRole != null)
            {
                userInRole.IsActive = true;
            }
            else
            {
                UserRole ur = new();
                ur.RoleId = role.Id;
                ur.UserId = user.Id;
                ur.IsActive = true;
                _context.UserRoles.Add(ur);
            }
        }

        await _context.SaveChangesAsync();
    }
}
