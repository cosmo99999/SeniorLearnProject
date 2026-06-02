using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using SeniorLearnProject.Data;
using SeniorLearnProject.Services;

namespace SeniorLearnProject.Models.Identity;

public class ActiveRolePolicy : IAuthorizationRequirement { }

public class ActiveRoleHandler : AuthorizationHandler<ActiveRolePolicy>
{
    private readonly UserService _uService;
    public ActiveRoleHandler(UserService uService)
    {
        _uService = uService;
    }
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveRolePolicy requirement)
    {
        var roles = context.Requirements.OfType<RolesAuthorizationRequirement>();
        var allowedroles = roles.SelectMany(r => r.AllowedRoles);
        var hasActiveRole = await _uService.DoesUserHaveActiveRole(context.User, "admin");

        if (hasActiveRole)
        {
            context.Succeed(requirement);
            return;
        }
        context.Fail();
    }
}
