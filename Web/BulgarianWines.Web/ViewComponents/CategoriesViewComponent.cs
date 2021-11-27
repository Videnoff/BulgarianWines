namespace BulgarianWines.Web.ViewComponents
{
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.categoriesService.GetAll<CategoryViewModel>();
            return this.View(categories);
        }
    }
}
