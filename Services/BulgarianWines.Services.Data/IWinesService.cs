namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IWinesService
    {
        Task CreateAsync<T>(T input, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        int GetCount();

        T GetById<T>(int id);

        public Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> RestoreAsync(int id);

        public Task<bool> DeleteImageAsync(string id);
    }
}
