namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.HomePageSlides;
    using BulgarianWines.Web.ViewModels.HomePage;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomePageSlidesController : AdministrationController
    {
        private readonly IHomePageSlidesService homePageSlidesService;
        private readonly IDeletableEntityRepository<HomePageSlide> homePageSlidesRepository;

        public HomePageSlidesController(
            IHomePageSlidesService homePageSlidesService,
            IDeletableEntityRepository<HomePageSlide> homePageSlidesRepository)
        {
            this.homePageSlidesService = homePageSlidesService;
            this.homePageSlidesRepository = homePageSlidesRepository;
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.homePageSlidesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
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

            var editResult = await this.homePageSlidesService.EditAsync(model, model.UploadedImages);
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

        public async Task<IActionResult> Restore(int id)
        {
            var restoreResult = await this.homePageSlidesService.RestoreAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored slide";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the slide";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            var result = await this.homePageSlidesService.DeleteImageAsync(id);

            if (result)
            {
                this.TempData["Alert"] = "Successfully deleted image!";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the image!";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Deleted()
        {
            var categories = this.homePageSlidesService.GetAllDeleted<DeletedHomePageSlidesViewModel>();
            return this.View(categories);
        }
    }
}
