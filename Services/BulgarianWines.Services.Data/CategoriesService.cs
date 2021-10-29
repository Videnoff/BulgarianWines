namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private const string AzureContainerName = "publicimages";

        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<CategoryImage> categoryImagesRepository;
        private readonly IImagesService imagesService;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IImagesService imagesService,
            IDeletableEntityRepository<CategoryImage> categoryImagesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.imagesService = imagesService;
            this.categoryImagesRepository = categoryImagesRepository;
        }

        public async Task CreateAsync<T>(T model, IEnumerable<IFormFile> images)
        {
            var category = AutoMapperConfig.MapperInstance.Map<Category>(model);

            if (images != null && images.Any())
            {
                foreach (var formFile in images)
                {
                    category.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(formFile, AzureContainerName);
                    var imageUrl = category.ImageUrl;
                    category.CategoryImages.Add(new CategoryImage
                    {
                        ImageUrl = imageUrl,
                    });
                }
            }

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images)
        {
            var newCategory = AutoMapperConfig.MapperInstance.Map<Category>(model);

            var foundCategory = this.GetById(newCategory.Id);

            if (foundCategory == null)
            {
                return false;
            }

            foundCategory.Description = newCategory.Description;
            foundCategory.Icon = newCategory.Icon;

            if (images != null && images.Count() > 0)
            {
                foreach (var image in images)
                {
                    foundCategory.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
                    var imageUrl = foundCategory.ImageUrl;
                    foundCategory.CategoryImages.Add(new CategoryImage
                    {
                        ImageUrl = imageUrl,
                    });
                }
            }

            this.categoriesRepository.Update(foundCategory);
            await this.categoriesRepository.SaveChangesAsync();

            return true;
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

        public async Task<bool> DeleteImageAsync(string id)
        {
            var image = this.GetImageById(id);

            if (image == null)
            {
                return false;
            }

            this.categoryImagesRepository.Delete(image);
            await this.categoryImagesRepository.SaveChangesAsync();

            return true;
        }

        private Category GetDeletedCategoryById(int id) =>
            this.categoriesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);

        private Category GetById(int id) =>
            this.categoriesRepository
                .All()
                .Include(x => x.CategoryImages)
                .FirstOrDefault(x => x.Id == id);

        private CategoryImage GetImageById(string id) =>
            this.categoryImagesRepository.All()
                .FirstOrDefault(x => x.Id == id);
    }
}
