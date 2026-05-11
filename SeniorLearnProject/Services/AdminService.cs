using System;
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Services;

public class AdminService
{
    private readonly SeniorLearnContext _context;

    public AdminService(SeniorLearnContext context)
    {
        _context = context;
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
}
