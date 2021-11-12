namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        public Task<bool> AddAsync(int wineId, string userId);

        public Task<bool> DeleteAsync(int wineId, string userId);

        public IEnumerable<T> GetAll<T>(string userId);
    }
}
