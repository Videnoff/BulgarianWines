namespace BulgarianWines.Web.ViewModels.Orders
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class OrderWinesViewModel : IMapFrom<WineOrder>, IHaveCustomMappings
    {
        public string WineId { get; set; }

        public string WineName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string WineProductCode { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.Price;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<WineOrder, OrderWinesViewModel>()
                .ForMember(
                    x => x.ImageUrl,
                    opt => opt.MapFrom(x => x.Wine.ImageUrl))
                .ForMember(
                    x => x.WineId,
                    opt => opt.MapFrom(x => x.WineId))
                .ForMember(
                    x => x.WineName,
                    opt => opt.MapFrom(x => x.Wine.Name))
                .ForMember(
                    x => x.WineProductCode,
                    opt => opt.MapFrom(x => x.Wine.ProductCode));
        }
    }
}
