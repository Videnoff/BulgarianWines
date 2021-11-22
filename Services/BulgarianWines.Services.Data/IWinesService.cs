namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IWinesService
    {
        Task CreateAsync<T>(T input, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        public IEnumerable<T> GetAllByCategoryId<T>(int categoryId, int page, int productsToTake, string sorting);

        public IEnumerable<T> GetBySearchTerm<T>(string searchTerm, int? categoryId, int page, int productsToTake, string sorting);

        public IEnumerable<T> GetNewest<T>(int productsToTake);

        public IEnumerable<T> GetNewestByCategory<T>(int productsToTake, int categoryId);

        int GetCount();

        T GetById<T>(int id);

        public Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> RestoreAsync(int id);

        public IEnumerable<T> GetAllDeleted<T>();

        public Task<bool> DeleteImageAsync(string id);

        public Task<bool> CreateReviewAsync<T>(T model);
    }
}
