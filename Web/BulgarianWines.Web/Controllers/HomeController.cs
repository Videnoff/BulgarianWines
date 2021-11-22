namespace BulgarianWines.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels;
    using BulgarianWines.Web.ViewModels.Administration.Categories;
    using BulgarianWines.Web.ViewModels.HomePage;
    using BulgarianWines.Web.ViewModels.Index;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    public class HomeController : BaseController
    {
        private const int DescriptionMaxLength = 40;

        private readonly IHomePageSlidesService homePageSlidesService;
        private readonly IDistributedCache distributedCache;

        private readonly ICategoriesService categoriesService;
        private readonly IWinesService winesService;
        private readonly IShortTextService shortTextService;

        public HomeController(
            IHomePageSlidesService homePageSlidesService,
            IDistributedCache distributedCache,
            ICategoriesService categoriesService,
            IWinesService winesService,
            IShortTextService shortTextService)
        {
            this.homePageSlidesService = homePageSlidesService;
            this.distributedCache = distributedCache;
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.shortTextService = shortTextService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModelAsString = await this.distributedCache.GetStringAsync("IndexViewModel");
            IndexViewModel viewModel;

            if (viewModelAsString == null)
            {
                var allCategories = this.categoriesService.GetAll<CategoryViewModel>();
                //var mostBoughtProducts = this.ordersService.GetMostBoughtProducts<ProductSidebarViewModel>(10);
                var newestProducts = this.winesService.GetNewest<ProductViewModel>(3);

                //var topRatedProducts = this.productsService.GetTopRated<ProductSidebarViewModel>(4);

                var slides = this.homePageSlidesService.GetAll<HomePageViewModel>();

                //foreach (var product in topRatedProducts)
                //{
                //    product.Name = this.stringService.TruncateAtWord(product.Name, 30);
                //}
                foreach (var product in newestProducts)
                {
                    product.Description = this.shortTextService.ShortText(product.Description, DescriptionMaxLength);
                }

                viewModel = new IndexViewModel
                {
                    //MostBoughtProducts = mostBoughtProducts,
                    NewestProducts = newestProducts,
                    //TopRatedProducts = topRatedProducts,
                    AllCategories = allCategories,
                    Slides = slides,
                };

                await this.distributedCache.SetStringAsync(
                    "IndexViewModel",
                    JsonConvert.SerializeObject(viewModel),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
                    });
            }
            else
            {
                viewModel = JsonConvert.DeserializeObject<IndexViewModel>(viewModelAsString);
            }

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult About()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
