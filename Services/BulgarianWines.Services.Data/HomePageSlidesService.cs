using Microsoft.EntityFrameworkCore;

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

        private readonly IDeletableEntityRepository<HomePageSlide> homePageSlidesRepository;
        private readonly IDeletableEntityRepository<SlideImage> slideImagesRepository;
        private readonly IImagesService imagesService;

        public HomePageSlidesService(
            IDeletableEntityRepository<HomePageSlide> homePageSlidesRepository,
            IImagesService imagesService,
            IDeletableEntityRepository<SlideImage> slideImagesRepository)
        {
            this.homePageSlidesRepository = homePageSlidesRepository;
            this.imagesService = imagesService;
            this.slideImagesRepository = slideImagesRepository;
        }

        public async Task CreateAsync<T>(T model, IEnumerable<IFormFile> images)
        {
            var slide = AutoMapperConfig.MapperInstance.Map<HomePageSlide>(model);

            if (images != null && images.Any())
            {
                foreach (var formFile in images)
                {
                    slide.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(formFile, AzureContainerName);
                    var imageUrl = slide.ImageUrl;
                    slide.SlideImages.Add(new SlideImage
                    {
                        ImageUrl = imageUrl,
                    });
                }
            }

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

            foreach (var image in slide.SlideImages)
            {
                this.slideImagesRepository.Delete(image);
            }

            await this.homePageSlidesRepository.SaveChangesAsync();
            await this.slideImagesRepository.SaveChangesAsync();

            return true;
        }

        private HomePageSlide GetById(int id) =>
            this.homePageSlidesRepository
                .All()
                .Include(x => x.SlideImages)
                .FirstOrDefault(x => x.Id == id);
    }
}
