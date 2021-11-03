namespace BulgarianWines.Web.ViewModels.Wines
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class ProductViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int OriginId { get; set; }

        public string OriginName { get; set; }

        public int VarietyId { get; set; }

        public string VarietyName { get; set; }

        public int HarvestId { get; set; }

        public string HarvestYear { get; set; }

        public int VolumeId { get; set; }

        public string VolumeQuantity { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int AvailabilityId { get; set; }

        public string AvailabilityStatus { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, ProductViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.Images.FirstOrDefault().ImageUrl != null
                            ? x.Images.FirstOrDefault().ImageUrl
                            : "/images/wines/" + x.Images.FirstOrDefault().Id + "." +
                              x.Images.FirstOrDefault().Extension));
        }
    }
}
