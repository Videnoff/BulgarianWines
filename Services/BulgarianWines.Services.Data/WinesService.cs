namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class WinesService : IWinesService
    {
        private const string AzureContainerName = "publicimages";

        private readonly IDeletableEntityRepository<Wine> winesRepository;
        private readonly IDeletableEntityRepository<Image> imagesRepository;
        private readonly IImagesService imagesService;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public WinesService(
            IDeletableEntityRepository<Wine> winesRepository,
            IDeletableEntityRepository<Image> imagesRepository,
            IImagesService imagesService,
            IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.winesRepository = winesRepository;
            this.imagesRepository = imagesRepository;
            this.imagesService = imagesService;
            this.reviewsRepository = reviewsRepository;
        }

        public async Task CreateAsync<T>(T input, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath)
        {
            var wine = AutoMapperConfig.MapperInstance.Map<Wine>(input);

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    wine.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
                    var imageUrl = wine.ImageUrl;

                    wine.Images.Add(new Image
                    {
                        ImageUrl = imageUrl,
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

        public IEnumerable<T> GetAllByCategoryId<T>(int categoryId, int page, int productsToTake, string sorting) =>
            this.winesRepository.AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .OrderBy(x => x.Name)
                .Skip((page - 1) * productsToTake)
                .Take(productsToTake)
                .To<T>().ToList();

        public IEnumerable<T> GetBySearchTerm<T>(string searchTerm, int? categoryId, int page, int productsToTake, string sorting)
        {
            var predicateExpression = this.BuildSearchPredicateExpression(searchTerm, categoryId);

            var columnName = string.Empty;
            var isAscending = true;

            sorting = sorting.ToLower();

            if (sorting == "price desc")
            {
                columnName = "Price";
                isAscending = false;
            }
            else if (sorting == "price asc")
            {
                columnName = "Price";
            }
            else if (sorting == "newest")
            {
                columnName = "CreatedOn";
                isAscending = false;
            }
            else if (sorting == "oldest")
            {
                columnName = "CreatedOn";
            }

            return this.winesRepository.AllAsNoTracking()
                .Where(predicateExpression)
                .Skip((page - 1) * productsToTake)
                .Take(productsToTake)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetNewest<T>(int productsToTake) => this.winesRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .Take(productsToTake)
            .To<T>()
            .ToList();

        public IEnumerable<T> GetNewestByCategory<T>(int productsToTake, int categoryId) => this.winesRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .Take(productsToTake)
            .Where(x => x.CategoryId == categoryId)
            .To<T>()
            .ToList();

        public int GetCount()
        {
            return this.winesRepository.All().Count();
        }

        public T GetById<T>(int id)
        {
            return this.winesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath)
        {
            var newProduct = AutoMapperConfig.MapperInstance.Map<Wine>(model);

            var foundProduct = this.GetById(newProduct.Id);

            if (foundProduct == null)
            {
                return false;
            }

            foundProduct.Name = newProduct.Name;
            foundProduct.ProductCode = newProduct.ProductCode;
            foundProduct.Description = newProduct.Description;
            foundProduct.HarvestId = newProduct.HarvestId;
            foundProduct.OriginId = newProduct.OriginId;
            foundProduct.VarietyId = newProduct.VarietyId;
            foundProduct.CategoryId = newProduct.CategoryId;
            foundProduct.VolumeId = newProduct.VolumeId;
            foundProduct.AvailabilityId = newProduct.AvailabilityId;
            foundProduct.Price = newProduct.Price;
            foundProduct.Price5To10 = newProduct.Price5To10;
            foundProduct.PriceAbove10 = newProduct.PriceAbove10;

            if (images != null && images.Count() > 0)
            {
                foreach (var image in images)
                {
                    var imageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);

                    foundProduct.Images.Add(new Image
                    {
                        ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/"),
                    });
                }
            }

            this.winesRepository.Update(foundProduct);
            await this.winesRepository.SaveChangesAsync();

            return true;
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

        public IEnumerable<T> GetAllDeleted<T>() => this.winesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>()
            .ToList();

        public async Task<bool> DeleteImageAsync(string id)
        {
            var image = this.GetImageById(id);

            if (image == null)
            {
                return false;
            }

            this.imagesRepository.Delete(image);
            await this.imagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateReviewAsync<T>(T model)
        {
            var wineReview = AutoMapperConfig.MapperInstance.Map<Review>(model);
            var wine = this.GetById(wineReview.WineId);

            if (wine == null || this.reviewsRepository.AllAsNoTracking().Any(x => x.UserId == wineReview.UserId))
            {
                return false;
            }

            await this.reviewsRepository.AddAsync(wineReview);
            await this.reviewsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(string id)
        {
            var review = this.GetReviewById(id);

            if (review == null)
            {
                return false;
            }

            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();

            return true;
        }

        public bool HasProduct(int id) => this.winesRepository.AllAsNoTracking().Any(x => x.Id == id);

        private Wine GetDeletedProductById(int id) =>
            this.winesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);

        private Wine GetById(int id) =>
            this.winesRepository.All().Include(x => x.Images)
                .FirstOrDefault(x => x.Id == id);

        private Image GetImageById(string id) =>
            this.imagesRepository.All()
                .FirstOrDefault(x => x.Id == id);

        private Expression<Func<Wine, bool>> BuildSearchPredicateExpression(string search, int? categoryId)
        {
            Expression<Func<Wine, bool>> predicateExpression = x => x.Name
                .ToLower()
                .Contains(search.ToLower());

            if (categoryId != null)
            {
                predicateExpression = x => x.Name
                    .ToLower()
                    .Contains(search.ToLower()) && x.CategoryId == categoryId;
            }

            return predicateExpression;
        }

        private Review GetReviewById(string id) => this.reviewsRepository
            .All()
            .FirstOrDefault(x => x.Id == id);
    }
}
