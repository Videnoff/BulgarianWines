using System;

namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class AllWinesViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public double AverageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, AllWinesViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.Images.FirstOrDefault().ImageUrl != null
                            ? x.Images.FirstOrDefault().ImageUrl
                            : "/images/wines/" + x.Images.FirstOrDefault().Id + "." +
                              x.Images.FirstOrDefault().Extension))
                .ForMember(
                    x => x.AverageRating,
                    opt => opt.MapFrom(m => (!m.Reviews.Any()) ? 0 : Math.Round(m.Reviews.Average(x => x.Rating), 2)));
        }
    }
}
