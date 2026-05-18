using SeniorLearnProject.Data;
using SeniorLearnProject.Models.Identity;
using System.Security.Claims;

namespace SeniorLearnProject.Services;

public class UserManagementService
{
    private readonly SeniorLearnContext _context;

    public UserManagementService(SeniorLearnContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesUserHaveActiveRole(ClaimsPrincipal claim, string role)
    {
        return true;
    }
}
