namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface IOriginsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
