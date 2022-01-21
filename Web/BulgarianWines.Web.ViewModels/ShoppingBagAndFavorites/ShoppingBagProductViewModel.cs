using System.Linq;
using AutoMapper;
using BulgarianWines.Data.Models;
using BulgarianWines.Services.Mapping;

namespace BulgarianWines.Web.ViewModels.ShoppingBagAndFavorites
{
    public class ShoppingBagProductViewModel : IMapFrom<ShoppingCartProduct>
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public double AverageRating { get; set; }

        [IgnoreMap]
        //public decimal TotalPrice => this.Quantity * this.ProductPrice;

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<ShoppingCartProduct, ShoppingBagProductViewModel>()
        //        .ForMember(
        //            x => x.ImageUrl, opt => opt.MapFrom(x => x.WineId.Images.FirstOrDefault().RemoteImageUrl != null
        //                ? x.WineId.Images.FirstOrDefault().RemoteImageUrl
        //                : "/images/products/" + x.Wine.Images.FirstOrDefault().Id + "." + x.Wine.Images.FirstOrDefault().Extension));
        }
    }
}