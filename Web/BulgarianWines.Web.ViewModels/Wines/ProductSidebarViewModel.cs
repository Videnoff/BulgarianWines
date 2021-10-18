namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class ProductSidebarViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, ProductSidebarViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.Images.FirstOrDefault().ImageUrl != null
                            ? x.Images.FirstOrDefault().ImageUrl
                            : "/images/wines/" + x.Images.FirstOrDefault().Id + "." +
                              x.Images.FirstOrDefault().Extension));
        }
    }
}
