namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAllDeleted<T>();

        public IEnumerable<Category> GetAll();

        public T GetById<T>(int id);

        public Task<bool> RestoreAsync(int id);
    }
}
