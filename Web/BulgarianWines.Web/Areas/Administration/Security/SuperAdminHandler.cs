namespace BulgarianWines.Web.Areas.Administration.Security
{
    using System;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using Microsoft.AspNetCore.Authorization;

    public class SuperAdminHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            if (context.User.IsInRole(GlobalConstants.SuperAdministratorRoleName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
