namespace BulgarianWines.Web.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.ShoppingCartAndFavorites;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsCountViewComponent : ViewComponent
    {
        private readonly IFavoritesService favoritesService;
        private readonly IShoppingCartService shoppingCartService;

        public ProductsCountViewComponent(
            IFavoritesService favoritesService,
            IShoppingCartService shoppingCartService)
        {
            this.favoritesService = favoritesService;
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoritesCount = this.favoritesService.GetCount(userId);
            var shoppingCArtProductsCount =
                await this.shoppingCartService.GetProductsCountAsync(this.User.Identity.IsAuthenticated, this.HttpContext.Session, userId);

            var model = new ShoppingCartAndFavoritesViewModel
            {
                ShoppingCartProductsCount = shoppingCArtProductsCount,
                FavoritesProductsCount = favoritesCount,
            };

            return this.View(model);
        }
    }
}
