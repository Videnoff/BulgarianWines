namespace BulgarianWines.Web.ViewModels.Orders
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class OrderProductsViewModel : IMapFrom<WineOrder>, IHaveCustomMappings
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.Price;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<WineOrder, OrderProductsViewModel>()
                .ForMember(
                    x => x.ImageUrl,
                    opt => opt.MapFrom(m => (!m.Wine.Images.Any())));
        }
    }
}
