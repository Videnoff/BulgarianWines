namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class AvailabilityService : IAvailabilityService
    {
        private readonly IRepository<Availability> availabilitysRepository;

        public AvailabilityService(IRepository<Availability> availabilitysRepository)
        {
            this.availabilitysRepository = availabilitysRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.availabilitysRepository
                .AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                })
                .OrderBy(x => x.Status)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Status));
        }
    }
}
