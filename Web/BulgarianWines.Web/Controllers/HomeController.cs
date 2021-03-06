namespace BulgarianWines.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels;
    using BulgarianWines.Web.ViewModels.Administration.Categories;
    using BulgarianWines.Web.ViewModels.HomePage;
    using BulgarianWines.Web.ViewModels.Index;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    public class HomeController : BaseController
    {
        private const int DescriptionMaxLength = 40;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IHomePageSlidesService homePageSlidesService;
        private readonly IDistributedCache distributedCache;

        private readonly ICategoriesService categoriesService;
        private readonly IWinesService winesService;
        private readonly IShortTextService shortTextService;
        private readonly IUserMessagesService userMessagesService;

        public HomeController(
            IHomePageSlidesService homePageSlidesService,
            IDistributedCache distributedCache,
            ICategoriesService categoriesService,
            IWinesService winesService,
            IShortTextService shortTextService,
            IUserMessagesService userMessagesService,
            UserManager<ApplicationUser> userManager)
        {
            this.homePageSlidesService = homePageSlidesService;
            this.distributedCache = distributedCache;
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.shortTextService = shortTextService;
            this.userMessagesService = userMessagesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var viewModelAsString = await this.distributedCache.GetStringAsync("IndexViewModel");
            IndexViewModel viewModel;

            if (viewModelAsString == null)
            {
                var allCategories = this.categoriesService.GetAll<CategoryViewModel>();

                // var mostBoughtProducts = this.ordersService.GetMostBoughtProducts<ProductSidebarViewModel>(10);
                var newestProducts = this.winesService.GetNewest<ProductViewModel>(3);

                // var topRatedProducts = this.productsService.GetTopRated<ProductSidebarViewModel>(4);

                var slides = this.homePageSlidesService.GetAll<HomePageViewModel>();

                // foreach (var product in topRatedProducts)
                // {
                //    product.Name = this.stringService.TruncateAtWord(product.Name, 30);
                // }
                foreach (var product in newestProducts)
                {
                    product.Description = this.shortTextService.ShortText(product.Description, DescriptionMaxLength);
                }

                viewModel = new IndexViewModel
                {
                    // MostBoughtProducts = mostBoughtProducts,
                    NewestProducts = newestProducts,

                    // TopRatedProducts = topRatedProducts,
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

        public IActionResult TermsAndConditions()
        {
            return this.View();
        }

        public IActionResult DeliveryInformation()
        {
            return this.View();
        }

        public IActionResult Return()
        {
            return this.View();
        }

        public async Task<IActionResult> Contact()
        {
            var userIdentity = this.User.Identity;
            if (userIdentity != null)
            {
                var userName = userIdentity.Name;
                if (userName != null)
                {

                    var user = await this.userManager.FindByNameAsync(userName);
                    var email = await this.userManager.GetEmailAsync(user);

                    if (userIdentity.IsAuthenticated)
                    {
                        ContactFormViewModel model = new ContactFormViewModel() {Email = email,};

                        return this.View(model);
                    }
                }
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.TempData["Alert"] = "Thank you! Your request was sent successfully!";

            await this.userMessagesService.CreateAsync<ContactFormViewModel>(model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult StatusCodePage(int code)
        {
            this.ViewData["StatusCode"] = code;
            var statusCodeFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeFeature.OriginalPath.Contains("/Administration"))
            {
                return this.RedirectToAction("StatusCodePage", "Dashboard", new
                {
                    code,
                    Area = "Administration",
                });
            }
            else
            {
                return this.View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
