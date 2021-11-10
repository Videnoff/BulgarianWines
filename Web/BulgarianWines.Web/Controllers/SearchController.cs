namespace BulgarianWines.Web.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Search;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : BaseController
    {
        private const int DescriptionMaxLength = 100;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };
        private readonly List<string> sortingValues = new List<string> { "Price asc", "Price desc", "Newest", "Oldest" };

        private readonly ICategoriesService categoriesService;
        private readonly IWinesService winesService;
        private readonly IShortTextService shortTextService;

        public SearchController(
            ICategoriesService categoriesService,
            IWinesService winesService,
            IShortTextService shortTextService)
        {
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.shortTextService = shortTextService;
        }

        public IActionResult Index(string searchTerm, int? categoryId = null, int pageNumber = 1, int itemsPerPage = 6, string sorting = "price asc")
        {
            if (pageNumber <= 0)
            {
                this.TempData["Error"] = "Page number cannot be negative.";
                return this.RedirectToAction("Index", "Home");
            }

            if (itemsPerPage <= 0)
            {
                this.TempData["Error"] = "Items per page cannot be negative.";
                return this.RedirectToAction("Index", "Home");
            }

            var products = this.winesService.GetBySearchTerm<ProductViewModel>(searchTerm, categoryId, pageNumber, itemsPerPage, sorting);

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var descriptionText = WebUtility.HtmlDecode(Regex.Replace(product.Description, @"<[^>]+>", string.Empty));
                    product.Description = this.shortTextService.ShortText(descriptionText, DescriptionMaxLength);
                }
            }

            var searchViewModel = new SearchProductInputModel
            {
                //ItemsCount = this.productsService.GetProductsCountBySearchStringAndMainCategoryId(searchTerm, mainCategoryId),
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Products = products,
                SortingValues = this.sortingValues,
                Sorting = sorting,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                ItemsPerPageValues = this.itemsPerPageValues,
                Area = string.Empty,
                Controller = "Search",
                Action = nameof(this.Index),
            };

            return this.View(searchViewModel);
        }
    }
}
