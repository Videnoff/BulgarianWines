namespace BulgarianWines.Web.Areas.Administration.Security
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    public class CanEditOnlyAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanEditOnlyAdminRolesAndClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor =
                httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var loggedInAdminId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            string adminBeingEdited = this.httpContextAccessor.HttpContext?.Request.Query["userId"];

            if (context.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                context.User.HasClaim(x => x.Type == "Edit Role" && x.Value == "true") &&
                adminBeingEdited.ToLower() != loggedInAdminId?.ToLower())
            {
                context.Succeed(requirement);
            }

            if (context.User.IsInRole(GlobalConstants.SuperAdministratorRoleName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
