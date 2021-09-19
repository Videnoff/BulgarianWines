namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class VolumesService : IVolumesService
    {
        private readonly IDeletableEntityRepository<Volume> volumesRepository;

        public VolumesService(IDeletableEntityRepository<Volume> volumesRepository)
        {
            this.volumesRepository = volumesRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.volumesRepository
                .AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Quantity,
                })
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Quantity));
        }
    }
}
