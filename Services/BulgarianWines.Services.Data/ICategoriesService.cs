namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface ICategoriesService
    {
        public Task CreateAsync<T>(T model, IEnumerable<IFormFile> images);

        public Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAllDeleted<T>();

        public IEnumerable<Category> GetAll();

        public T GetById<T>(int id);

        public Task<bool> RestoreAsync(int id);

        public Task<bool> DeleteImageAsync(string id);
    }
}
