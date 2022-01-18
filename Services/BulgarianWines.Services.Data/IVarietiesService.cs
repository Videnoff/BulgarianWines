namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public interface IVarietiesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        public Task CreateAsync<T>(T model);

        public Task<bool> EditAsync<T>(T model);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAllDeleted<T>();

        public IEnumerable<Variety> GetAll();

        public T GetById<T>(int id);

        public Task<bool> RestoreAsync(int id);
    }
}
