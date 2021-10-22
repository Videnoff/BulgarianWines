namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.categoriesRepository
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

        public IEnumerable<T> GetAll<T>() => this.categoriesRepository.AllAsNoTracking().To<T>().ToList();

        public IEnumerable<T> GetAllDeleted<T>() => this.categoriesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>()
            .ToList();

        public IEnumerable<Category> GetAll() => this.categoriesRepository.AllAsNoTracking().ToList();

        public async Task<bool> RestoreAsync(int id)
        {
            var category = this.GetDeletedCategoryById(id);

            if (category == null)
            {
                return false;
            }

            this.categoriesRepository.Undelete(category);
            await this.categoriesRepository.SaveChangesAsync();

            return true;
        }

        private Category GetDeletedCategoryById(int id) =>
            this.categoriesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);
    }
}
