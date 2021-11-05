namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class SingleProductViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public string Origin { get; set; }

        public string OriginName { get; set; }

        public string Harvest { get; set; }

        public string HarvestYear { get; set; }

        public string Variety { get; set; }

        public string VarietyName { get; set; }

        public string Description { get; set; }

        public IEnumerable<Image> WineImages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, SingleProductViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.Images.FirstOrDefault().ImageUrl != null
                            ? x.Images.FirstOrDefault().ImageUrl
                            : "/images/wines/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault()))
                .ForMember(
                    x => x.WineImages,
                    opt => opt.MapFrom(x => x.Images));
        }
    }
}
