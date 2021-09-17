namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using BulgarianWines.Common;
    using BulgarianWines.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
