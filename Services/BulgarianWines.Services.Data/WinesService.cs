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
                Harvest = input.Harvest,
                VolumeId = input.VolumeId,
                Origin = input.Origin,
                Variety = input.Variety,
            };

            await this.winesRepository.AddAsync(wine);
            await this.winesRepository.SaveChangesAsync();
        }
    }
}
