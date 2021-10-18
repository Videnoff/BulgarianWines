namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class HomePageSlidesService : IHomePageSlidesService
    {
        private const string AzureContainerName = "publicimages";

        private readonly IRepository<HomePageSlide> homePageSlidesRepository;
        private readonly IImagesService imagesService;

        public HomePageSlidesService(
            IRepository<HomePageSlide> homePageSlidesRepository,
            IImagesService imagesService)
        {
            this.homePageSlidesRepository = homePageSlidesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T model, IFormFile image)
        {
            var slide = AutoMapperConfig.MapperInstance.Map<HomePageSlide>(model);

            slide.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);

            await this.homePageSlidesRepository.AddAsync(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() =>
            this.homePageSlidesRepository.AllAsNoTracking()
                .OrderBy(x => x.Position)
                .To<T>().ToList();

        public T GetById<T>(int id) =>
            this.homePageSlidesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public async Task<bool> EditAsync<T>(T model, IFormFile image)
        {
            var newSlide = AutoMapperConfig.MapperInstance.Map<HomePageSlide>(model);

            var foundSlide = this.GetById(newSlide.Id);
            if (foundSlide == null)
            {
                return false;
            }

            foundSlide.Description = newSlide.Description;
            foundSlide.LinkUrl = newSlide.LinkUrl;

            if (image != null)
            {
                foundSlide.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
            }

            this.homePageSlidesRepository.Update(foundSlide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var slide = this.GetById(id);
            if (slide == null)
            {
                return false;
            }

            this.homePageSlidesRepository.Delete(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        private HomePageSlide GetById(int id) =>
            this.homePageSlidesRepository.All()
                .FirstOrDefault(x => x.Id == id);
    }
}
