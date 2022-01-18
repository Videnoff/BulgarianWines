namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

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
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task CreateAsync<T>(T model)
        {
            var variety = AutoMapperConfig.MapperInstance.Map<Variety>(model);

            await this.varietiesRepository.AddAsync(variety);
            await this.varietiesRepository.SaveChangesAsync();
        }

        public async Task<bool> EditAsync<T>(T model)
        {
            var newVariety = AutoMapperConfig.MapperInstance.Map<Variety>(model);

            var foundVariety = this.GetById(newVariety.Id);

            if (foundVariety == null)
            {
                return false;
            }

            this.varietiesRepository.Update(foundVariety);
            await this.varietiesRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAll<T>() => this.varietiesRepository.AllAsNoTracking().To<T>().ToList();

        public IEnumerable<T> GetAllDeleted<T>() => this.varietiesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>()
            .ToList();

        public IEnumerable<Variety> GetAll() => this.varietiesRepository.AllAsNoTracking().ToList();

        public T GetById<T>(int id) =>
            this.varietiesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public async Task<bool> RestoreAsync(int id)
        {
            var variety = this.GetDeletedVarietyById(id);

            if (variety == null)
            {
                return false;
            }

            this.varietiesRepository.Undelete(variety);
            await this.varietiesRepository.SaveChangesAsync();

            return true;
        }

        private Variety GetDeletedVarietyById(int id) =>
            this.varietiesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);

        private Variety GetById(int id) =>
            this.varietiesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
    }
}
