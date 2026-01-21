using Microsoft.AspNetCore.Authorization;

namespace EnterpriseBaseline.Api.Authorization
{
    public class PermissionAuthorizationHandler
         : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
            {
                if (context.User?.Identity?.IsAuthenticated != true)
                    return Task.CompletedTask;

                var permissions = context.User.FindAll("permission")
                                              .Select(c => c.Value);

                if (permissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
    }
}
