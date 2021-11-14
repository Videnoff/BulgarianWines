using System.Security.Claims;
using System.Threading.Tasks;
using BulgarianWines.Services.Data;
using BulgarianWines.Web.ViewModels.ShoppingBagAndFavorites;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianWines.Web.ViewComponents
{
    public class ProductsCountViewComponent : ViewComponent
    {
        private readonly IFavoritesService favoritesService;

        public ProductsCountViewComponent(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoritesCount = this.favoritesService.GetCount(userId);

            var model = new ShoppingBagAndFavoritesViewModel
            {
                FavoriteProductsCount = favoritesCount,
            };

            return this.View(model);
        }
    }
}
