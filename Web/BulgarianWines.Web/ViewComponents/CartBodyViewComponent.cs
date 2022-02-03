namespace BulgarianWines.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Mvc;

    public class CartBodyViewComponent : ViewComponent
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IShortTextService shortTextService;

        public CartBodyViewComponent(
            IShoppingCartService shoppingCartService,
            IShortTextService shortTextService)
        {
            this.shoppingCartService = shoppingCartService;
            this.shortTextService = shortTextService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await this.shoppingCartService.GetAllProductsAsync<ShoppingCartProductViewModel>(this.User.Identity.IsAuthenticated, this.HttpContext.Session, userId);

            if (products != null)
            {
                foreach (var product in products)
                {
                    product.ProductName = this.shortTextService.ShortText(product.ProductName, 15);
                }
            }

            var viewModel = new ShoppingCartViewModel
            {
                Products = products ?? new List<ShoppingCartProductViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
