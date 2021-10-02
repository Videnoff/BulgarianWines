using System.Threading.Tasks;

namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        public Task<bool> RestoreAsync(int id);
    }
}
