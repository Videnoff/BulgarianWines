namespace BulgarianWines.Web.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : Controller
    {
        private readonly IWinesService winesService;

        public WinesController(IWinesService winesService)
        {
            this.winesService = winesService;
        }

        public IActionResult AllWines(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int itemsPerPage = 8;

            var viewModel = new WinesListViewModel
            {
                ItemsPerPage = itemsPerPage,
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
