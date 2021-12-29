using System.Collections.Generic;

namespace BulgarianWines.Services
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        public T GetById<T>(string id);

        // public string GetImage();

        public Task<bool> DeleteAsync(string id);

        public Task<bool> RestoreAsync(string id);

        public IEnumerable<T> GetAllDeleted<T>();
    }
}