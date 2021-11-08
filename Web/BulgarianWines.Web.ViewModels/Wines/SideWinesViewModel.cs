namespace BulgarianWines.Web.ViewModels.Wines
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class SideWinesViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            //configuration.CreateMap<Wine, SideWinesViewModel>()
            //    .ForMember(
            //        source => source.AverageRating,
            //        destination => destination.MapFrom(member => (!member.Reviews.Any()) ? 0 : Math.Round(member.Reviews.Average(x => x.Rating), 2)))
            //    .ForMember(
            //        source => source.ImageUrl,
            //        destination => destination.MapFrom(member => (!member.Images.Any()) ? GlobalConstants.ImageNotFoundPath : member.Images.FirstOrDefault().ImageUrl));
        }
    }
}
