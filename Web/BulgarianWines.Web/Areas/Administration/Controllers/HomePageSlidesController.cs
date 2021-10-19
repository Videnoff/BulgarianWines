namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.HomePageSlides;
    using BulgarianWines.Web.ViewModels.HomePage;
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

            await this.homePageSlidesService.CreateAsync(model, model.UploadedImages);

            this.TempData["Alert"] = "Successfully created slide.";

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Index()
        {
            var products = this.homePageSlidesService.GetAll<HomePageViewModel>();
            return this.View(products);
        }

        public IActionResult Edit(int id)
        {
            var slide = this.homePageSlidesService.GetById<EditSlideViewModel>(id);
            if (slide == null)
            {
                this.TempData["Error"] = "Slide not found.";
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(slide);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSlideViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var editResult = await this.homePageSlidesService.EditAsync(model, model.Image);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the slide.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.homePageSlidesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the slide.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
