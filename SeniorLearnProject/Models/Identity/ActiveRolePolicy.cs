using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using SeniorLearnProject.Data;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Models.Identity;

public class ActiveRolePolicy : IAuthorizationRequirement { }

public class ActiveRoleHandler : AuthorizationHandler<ActiveRolePolicy>
{
    private readonly UserManagementService _uService;
    public ActiveRoleHandler(UserManagementService uService)
    {
        _uService = uService;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveRolePolicy requirement)
    {
        var roles = context.Requirements.OfType<RolesAuthorizationRequirement>().ToList();
        bool hasActiveRole = _uService.DoesUserHaveActiveRole(context.User, "roles").Result;
        return Task.CompletedTask;
    }
}
