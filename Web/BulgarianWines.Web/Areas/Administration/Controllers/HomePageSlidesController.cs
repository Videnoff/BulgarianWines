using BulgarianWines.Web.ViewModels.Administration.HomePageSlides;
using BulgarianWines.Web.ViewModels.HomePage;

namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class HomePageSlidesController : AdministrationController
    {
        private readonly IHomePageSlidesService homePageSlidesService;

        public HomePageSlidesController(IHomePageSlidesService homePageSlidesService)
        {
            this.homePageSlidesService = homePageSlidesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.homePageSlidesService.CreateAsync(model, model.Image);

            this.TempData["Alert"] = "Successfully created slide.";

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Index()
        {
            var products = this.homePageSlidesService.GetAll<HomePageViewModel>();
            return this.View(products);
        }
    }
}
