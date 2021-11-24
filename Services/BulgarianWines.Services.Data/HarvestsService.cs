namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class HarvestsService : IHarvestsService
    {
        private readonly IDeletableEntityRepository<Harvest> harvestsRepository;

        public HarvestsService(IDeletableEntityRepository<Harvest> harvestsRepository)
        {
            this.harvestsRepository = harvestsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.harvestsRepository
                .AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                })
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Year));
        }
    }
}
