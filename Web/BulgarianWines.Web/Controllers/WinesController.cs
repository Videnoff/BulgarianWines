namespace BulgarianWines.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : BaseController
    {
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };

        private readonly IWinesService winesService;
        private readonly ApplicationDbContext dbContext;

        public WinesController(
            IWinesService winesService,
            ApplicationDbContext dbContext)
        {
            this.winesService = winesService;
            this.dbContext = dbContext;
        }

        public IActionResult AllWines(int id = 1, int pageNumber = 1, int itemsPerPage = 8, string sorting = "price asc")
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            //const int itemsPerPage = 8;

            var viewModel = new WinesListViewModel
            {
                ItemsPerPage = itemsPerPage,
                ItemsPerPageValues = this.itemsPerPageValues,
                PageNumber = id,
                WinesCount = this.winesService.GetCount(),
                Wines = this.winesService.GetAll<AllWinesViewModel>(id, itemsPerPage),
            };

            return this.View(viewModel);
        }

        public IActionResult SingleWine(int id)
        {
            var wine = this.winesService.GetById<SingleProductViewModel>(id);

            if (wine == null)
            {
                this.TempData["Error"] = "Product not found.";
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(wine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(WineReviewInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.SingleWine), new { id = model.WineId });
            }

            var result = await this.winesService.CreateReviewAsync<WineReviewInputModel>(model);

            if (result)
            {
                this.TempData["Alert"] = "Successfully added wine review.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product review.";
            }

            return this.RedirectToAction(nameof(this.SingleWine), new { id = model.WineId });
        }

        public IActionResult ProductsFromCategory(int categoryId)
        {
            this.ViewBag.category = this.winesService.GetNewestByCategory<ProductViewModel>(3, categoryId);
            return this.PartialView("_ProductsFromCategoryPartial");
        }
    }
}
