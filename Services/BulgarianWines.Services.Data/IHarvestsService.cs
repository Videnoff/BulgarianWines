namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface IHarvestsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
