namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IUsersService usersService;

        public DashboardController(
            IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet("/Administration/StatusCodePage/{code}")]
        public IActionResult StatusCodePage(int code)
        {
            this.ViewData["StatusCode"] = code;
            return this.View();
        }
    }
}
