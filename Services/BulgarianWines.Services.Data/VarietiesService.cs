namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class VarietiesService : IVarietiesService
    {
        private readonly IDeletableEntityRepository<Variety> varietiesRepository;

        public VarietiesService(IDeletableEntityRepository<Variety> varietiesRepository)
        {
            this.varietiesRepository = varietiesRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.varietiesRepository
                .AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
