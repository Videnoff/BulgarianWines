using System.Collections.Generic;
using System.Linq;

namespace BulgarianWines.Services.Data
{
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Web.ViewModels.Wines;

    public class WinesService : IWinesService
    {
        private readonly IDeletableEntityRepository<Wine> winesRepository;

        public WinesService(IDeletableEntityRepository<Wine> winesRepository)
        {
            this.winesRepository = winesRepository;
        }

        public async Task CreateAsync(CreateWineInputModel input)
        {
            var wine = new Wine
            {
                Name = input.Name,
                CategoryId = input.CategoryId,
                Description = input.Description,
                HarvestId = input.HarvestId,
                VolumeId = input.VolumeId,
                OriginId = input.OriginId,
                VarietyId = input.VarietyId,
            };

            await this.winesRepository.AddAsync(wine);
            await this.winesRepository.SaveChangesAsync();
        }

        public IEnumerable<AllWinesViewModel> GetAll(int page, int itemsPerPage = 12)
        {
            var wines = this.winesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(x => new AllWinesViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryName = x.Category.Name,
                    CategoryId = x.CategoryId,
                    ImageUrl = x.Images.FirstOrDefault().RemoteImageUrl != null ?
                        x.Images.FirstOrDefault().RemoteImageUrl :
                        "/images/wines/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension,
                })
                .ToList();

            return wines;
        }
    }
}
