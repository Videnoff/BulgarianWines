namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IHomePageSlidesService
    {
        public Task CreateAsync<T>(T model, IEnumerable<IFormFile> images);

        public IEnumerable<T> GetAll<T>();

        public T GetById<T>(int id);

        public Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> DeleteImageAsync(string id);
    }
}
