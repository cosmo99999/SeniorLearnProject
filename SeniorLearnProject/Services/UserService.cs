using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Areas.Admin.Models;
using SeniorLearnProject.Data;
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
        var result = await _context.Users.FromSql(
        $"""
           SELECT * FROM AspNetUsers 
           WHERE UserName LIKE '%{name}%'
        """
        ).ToListAsync();
        return result;
    }
    // public List<Areas.Admin.Models.UserModel> ConvertUserToAdminUserModel(List<User> users)
    // {
    //     List<UserModel> result = new();
    //     foreach(var u in users)
    //     {
    //         UserModel m = new();
    //         m.Email = u.Email
    //     }
    // }
}
