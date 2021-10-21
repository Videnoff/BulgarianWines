namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IUsersService usersService;

        public DashboardController(
            ISettingsService settingsService,
            IUsersService usersService)
        {
            this.settingsService = settingsService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                ImageUrl = this.usersService.GetImage(),
            };

            return this.View(viewModel);
        }
    }
}
