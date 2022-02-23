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

        public decimal Price5To10 { get; set; }

        public decimal PriceAbove10 { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.Price;

        [IgnoreMap]
        public decimal TotalPriceAbove10 => this.Quantity * this.PriceAbove10;

        [IgnoreMap]
        public decimal TotalPrice5To10 => this.Quantity * this.Price5To10;

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
                    opt => opt.MapFrom(x => x.Wine.ProductCode))
                .ForMember(
                    x => x.Price5To10,
                    opt => opt.MapFrom(m => m.Wine.Price5To10))
                .ForMember(
                    x => x.PriceAbove10,
                    opt => opt.MapFrom(m => m.Wine.PriceAbove10));
        }
    }
}
