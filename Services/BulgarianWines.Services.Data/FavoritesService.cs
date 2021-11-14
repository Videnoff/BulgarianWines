namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class FavoritesService : IFavoritesService
    {
        private readonly IDeletableEntityRepository<Wine> winesRepository;
        private readonly IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository;
        private readonly UserManager<ApplicationUser> userManger;

        public FavoritesService(
            IDeletableEntityRepository<Wine> winesRepository,
            IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository,
            UserManager<ApplicationUser> userManger)
        {
            this.winesRepository = winesRepository;
            this.favoriteProductsRepository = favoriteProductsRepository;
            this.userManger = userManger;
        }

        public async Task<bool> AddAsync(int wineId, string userId)
        {
            var wine = this.GetWineById(wineId);

            if (wine == null)
            {
                return false;
            }

            var user = await this.userManger.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            await this.favoriteProductsRepository.AddAsync(new FavoriteProduct
            {
                WineId = wine.Id,
                UserId = user.Id,
            });

            await this.favoriteProductsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int wineId, string userId)
        {
            var wine = this.GetWineById(wineId);

            if (wine == null)
            {
                return false;
            }

            var user = await this.userManger.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var favoriteProduct = this.GetFavoriteProductById(wineId, userId);

            if (favoriteProduct == null)
            {
                return false;
            }

            this.favoriteProductsRepository.Delete(favoriteProduct);
            await this.favoriteProductsRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAll<T>(string userId) => this.favoriteProductsRepository
            .AllAsNoTracking()
            .Where(x => x.UserId == userId)
            .To<T>()
            .ToList();

        public int GetCount(string userId) =>
            this.favoriteProductsRepository
                .AllAsNoTracking()
                .Count(x => x.UserId == userId);

        private Wine GetWineById(int id) =>
            this.winesRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);

        private FavoriteProduct GetFavoriteProductById(int wineId, string userId) =>
            this.favoriteProductsRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.WineId == wineId && x.UserId == userId);
    }
}
