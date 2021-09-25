namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class OriginsService : IOriginsService
    {
        private readonly IDeletableEntityRepository<Origin> originsRepository;

        public OriginsService(IDeletableEntityRepository<Origin> originsRepository)
        {
            this.originsRepository = originsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.originsRepository
                .AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
