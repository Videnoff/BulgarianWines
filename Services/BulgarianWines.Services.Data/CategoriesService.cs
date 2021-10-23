namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class CategoriesService : ICategoriesService
    {
        private const string AzureContainerName = "publicimages";

        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IImagesService imagesService;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IImagesService imagesService)
        {
            this.categoriesRepository = categoriesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T model, IFormFile image)
        {
            var category = AutoMapperConfig.MapperInstance.Map<Category>(model);

            if (image != null)
            {
                    category.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
                    var imageUrl = category.ImageUrl;
                    category.CategoryImages.Add(new CategoryImage
                    {
                        ImageUrl = imageUrl,
                    });
            }

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
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

        public T GetById<T>(int id) => 
            this.categoriesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

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
