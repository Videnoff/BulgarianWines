using Microsoft.EntityFrameworkCore;

namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Http;

    public class WinesService : IWinesService
    {
        private const string AzureContainerName = "publicimages";

        private readonly IDeletableEntityRepository<Wine> winesRepository;
        private readonly IDeletableEntityRepository<Image> imagesRepository;
        private readonly IImagesService imagesService;

        public WinesService(
            IDeletableEntityRepository<Wine> winesRepository,
            IDeletableEntityRepository<Image> imagesRepository,
            IImagesService imagesService)
        {
            this.winesRepository = winesRepository;
            this.imagesRepository = imagesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T input, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath)
        {
            var wine = AutoMapperConfig.MapperInstance.Map<Wine>(input);

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    var imageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);

                    wine.Images.Add(new Image
                    {
                        ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/"),
                    });
                }
            }

            await this.winesRepository.AddAsync(wine);
            await this.winesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            var wines = this.winesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();

            return wines;
        }

        public int GetCount()
        {
            return this.winesRepository.All().Count();
        }

        public T GetById<T>(int id)
        {
            return this.winesRepository.AllAsNoTracking().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = this.GetById(id);

            if (product == null)
            {
                return false;
            }

            this.winesRepository.Delete(product);

            foreach (var image in product.Images)
            {
                this.imagesRepository.Delete(image);
            }

            await this.winesRepository.SaveChangesAsync();
            await this.imagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var product = this.GetDeletedProductById(id);

            if (product == null)
            {
                return false;
            }

            this.winesRepository.Undelete(product);
            await this.winesRepository.SaveChangesAsync();

            return true;
        }

        private Wine GetDeletedProductById(int id) =>
            this.winesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);

        private Wine GetById(int id) =>
            this.winesRepository.All().Include(x => x.Images)
                .FirstOrDefault(x => x.Id == id);
    }
}
