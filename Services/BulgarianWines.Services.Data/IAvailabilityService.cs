namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface IAvailabilityService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
