namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;

    public interface IVolumesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
