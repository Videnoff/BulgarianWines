namespace BulgarianWines.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

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

            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.isUserAuthenticated = contextAccessor.HttpContext.User.Identity.IsAuthenticated;
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

        [HttpGet("/ShoppingCart/Add/{productId:int}")]
        public async Task<IActionResult> Add(int productId)
        {
            var addResult =
                await this.shoppingCartService.AddProductAsync(this.isUserAuthenticated, this.session, this.userId, productId);

            if (addResult)
            {
                this.TempData["Alert"] = "Successfully added the product to the shopping cart.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product to the shopping cart.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet("/ShoppingCart/Delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var deleteResult = await this.shoppingCartService.DeleteProductAsync(this.isUserAuthenticated, this.session, this.userId, productId);

            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully removed the product from the shopping cart.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem removing the product from the shopping cart.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet("/ShoppingCart/Quantity/{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, bool increase)
        {
            var updateResult = await this.shoppingCartService.UpdateQuantityAsync(this.isUserAuthenticated, this.session, this.userId, productId, increase);

            if (updateResult)
            {
                this.TempData["Alert"] = "Successfully updated product quantity.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem updating the product quantity.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
