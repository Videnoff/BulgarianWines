namespace BulgarianWines.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using BulgarianWines.Data;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Categories;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : Controller
    {
        private const int DescriptionMaxLength = 100;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };
        private readonly List<string> sortingValues = new List<string> { "Price asc", "Price desc", "Newest", "Oldest" };

        private readonly ApplicationDbContext context;

        private readonly IWinesService winesService;
        private readonly ICategoriesService categoriesService;
        private readonly IShortTextService shortTextService;

        public CategoriesController(
            ApplicationDbContext context,
            IWinesService winesService,
            ICategoriesService categoriesService,
            IShortTextService shortTextService)
        {
            this.context = context;
            this.winesService = winesService;
            this.categoriesService = categoriesService;
            this.shortTextService = shortTextService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpGet("/Categories/{categoryId}")]
        public IActionResult Products(int categoryId, int pageNumber = 1, int itemsPerPage = 6, string sorting = "price asc")
        {
            if (pageNumber <= 0)
            {
                return this.Products(categoryId);
            }

            if (itemsPerPage <= 0)
            {
                return this.Products(categoryId);
            }

            var categoryNameAndProductCount = this.categoriesService.GetById<CategoryNameAndProductCountViewModel>(categoryId);
            if (categoryNameAndProductCount == null)
            {
                this.TempData["Error"] = "Category not found.";
                return this.RedirectToAction("Index", "Home");
            }

            var products = this.winesService.GetAllByCategoryId<ProductViewModel>(categoryId, pageNumber, itemsPerPage, sorting);

            foreach (var product in products)
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
                Products = products,
                ItemsPerPageValues = this.itemsPerPageValues,
                Sorting = sorting,
                SortingValues = this.sortingValues,
                //Area = string.Empty,
                //Controller = "Categories",
                //Action = nameof(this.Products),
            };

            return this.View(category);
        }

        private bool CategoryExists(int id)
        {
            return this.context.Categories.Any(e => e.Id == id);
        }
    }
}
