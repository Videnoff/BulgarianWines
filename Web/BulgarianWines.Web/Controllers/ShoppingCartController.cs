using System.Linq;
using System.Threading.Tasks;
using BulgarianWines.Services.Data;
using BulgarianWines.Web.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianWines.Web.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IWinesService winesService;

        private readonly string userId;
        private readonly bool isUserAuthenticated;
        private readonly ISession session;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IWinesService winesService,
            IHttpContextAccessor contextAccessor)
        {
            this.shoppingCartService = shoppingCartService;
            this.winesService = winesService;
            this.session = contextAccessor.HttpContext.Session;
        }

        public async Task<IActionResult> Index()
        {
            var shoppingCartProducts = await this.shoppingCartService
                .GetAllProductsAsync<ShoppingCartProductViewModel>(
                    this.isUserAuthenticated,
                    this.session,
                    this.userId);

            if (shoppingCartProducts == null || !shoppingCartProducts.Any())
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(new ShoppingCartViewModel()
            {
                Products = shoppingCartProducts,
            });
        }


    }
}