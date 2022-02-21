namespace BulgarianWines.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class ShoppingCartProductViewModel : IMapFrom<ShoppingCartProduct>, IHaveCustomMappings
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductCode { get; set; }

        public int Quantity { get; set; }

        public double AverageRating { get; set; }

        public decimal Price5To10 { get; set; }

        public decimal PriceAbove10 { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.ProductPrice;

        [IgnoreMap]
        public decimal TotalPriceAbove10 => this.Quantity * this.PriceAbove10;

        [IgnoreMap]
        public decimal TotalPrice5To10 => this.Quantity * this.Price5To10;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ShoppingCartProduct, ShoppingCartProductViewModel>()
                .ForMember(
                    x => x.AverageRating,
                    opt => opt.MapFrom(m =>
                        (!m.Wine.Reviews.Any()) ? 0 : Math.Round(m.Wine.Reviews.Average(x => x.Rating), 2)))
                .ForMember(
                    x => x.ImageUrl,
                    opt => opt.MapFrom(m => m.Wine.Images.FirstOrDefault().ImageUrl))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(m => m.Wine.Name))
                .ForMember(x => x.ProductPrice, opt => opt.MapFrom(m => m.Wine.Price))
                .ForMember(x => x.ProductId, opt => opt.MapFrom(m => m.Wine.Id))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(m => m.Wine.ProductCode))
                .ForMember(
                    x => x.Price5To10,
                    opt => opt.MapFrom(m => m.Wine.Price5To10))
                .ForMember(
                    x => x.PriceAbove10,
                    opt => opt.MapFrom(m => m.Wine.PriceAbove10));
        }
    }
}
