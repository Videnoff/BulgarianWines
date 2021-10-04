namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        public Task<bool> RestoreAsync(int id);
    }
}
