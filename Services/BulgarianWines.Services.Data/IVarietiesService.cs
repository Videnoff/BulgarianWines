namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface IVarietiesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
