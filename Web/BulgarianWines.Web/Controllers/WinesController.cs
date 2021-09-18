using BulgarianWines.Services.Data;

namespace BulgarianWines.Web.Controllers
{
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public WinesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateWineInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateWineInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            return this.Redirect("/");
        }
    }
}
