namespace BulgarianWines.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Categories;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class RelatedProductsViewComponent : ViewComponent
    {
        private const int DescriptionMaxLength = 100;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };
        private readonly List<string> sortingValues = new List<string> { "Price asc", "Price desc", "Newest", "Oldest" };

        private readonly IWinesService winesService;
        private readonly ICategoriesService categoriesService;
        private readonly IShortTextService shortTextService;

        public RelatedProductsViewComponent(
            IWinesService winesService,
            IShortTextService shortTextService,
            ICategoriesService categoriesService)
        {
            this.winesService = winesService;
            this.shortTextService = shortTextService;
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke(int categoryId, int pageNumber = 1, int itemsPerPage = 6, string sorting = "price asc")
        {
            var categoryNameAndProductCount = this.categoriesService.GetById<CategoryNameAndProductCountViewModel>(categoryId);

            var allProducts =
                this.winesService.GetAllByCategoryId<ProductViewModel>(categoryId, pageNumber, itemsPerPage, sorting);

            var relatedProducts = new List<SideWinesViewModel>();

            foreach (var product in allProducts)
            {
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var descriptionText = WebUtility.HtmlDecode(Regex.Replace(product.Description, @"<[^>]+>", string.Empty));
                    product.Description = this.shortTextService.ShortText(descriptionText, DescriptionMaxLength);
                }
            }

            var category = new CategoryProductsViewModel
            {
                Id = categoryNameAndProductCount.Id,
                Name = categoryNameAndProductCount.Name,
                //ItemsCount = categoryNameAndProductCount.ProductsCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Products = allProducts,
                ItemsPerPageValues = this.itemsPerPageValues,
                Sorting = sorting,
                SortingValues = this.sortingValues,
                //Area = string.Empty,
                //Controller = "Categories",
                //Action = nameof(this.Products),
            };

            return this.View((IEnumerable<SideWinesViewModel>)category);
        }
    }
}
