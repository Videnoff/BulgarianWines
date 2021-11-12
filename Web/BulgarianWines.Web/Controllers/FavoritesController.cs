namespace BulgarianWines.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Favorites;
    using Microsoft.AspNetCore.Mvc;

    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        public IActionResult All()
        {
            var favorites = this.favoritesService.GetAll<FavoriteProductViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            return this.View(favorites);
        }

        public async Task<IActionResult> Add(int id)
        {
            var result = await this.favoritesService.AddAsync(id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (result)
            {
                this.TempData["Alert"] = "Successfully added product to favourites.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product to favourites.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.favoritesService.DeleteAsync(id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (result)
            {
                this.TempData["Alert"] = "Successfully removed product from favourites.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem removing the product from favourites.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
