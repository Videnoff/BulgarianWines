namespace BulgarianWines.Web.ViewModels.Favorites
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class FavoriteProductViewModel : IMapFrom<FavoriteProduct>, IHaveCustomMappings
    {
        public int WineId { get; set; }

        public string WineName { get; set; }

        public decimal WinePrice { get; set; }

        public string ImageUrl { get; set; }

        public string WineAvailabilityStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FavoriteProduct, FavoriteProductViewModel>()
                .ForMember(
                    x => x.ImageUrl,
                    opt => opt.MapFrom(x => x.Wine.Images.FirstOrDefault().ImageUrl));
        }
    }
}
