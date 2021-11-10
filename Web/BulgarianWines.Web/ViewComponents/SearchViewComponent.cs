namespace BulgarianWines.Web.ViewComponents
{
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Category;
    using BulgarianWines.Web.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;

    public class SearchViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public SearchViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.categoriesService.GetAll<CategoryViewModel>();

            var model = new SearchInputViewModel
            {
                Categories = categories,
            };

            return this.View(model);
        }
    }
}
