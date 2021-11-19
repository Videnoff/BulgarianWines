namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Globalization;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class WineReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public byte Rating { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string UserEmail { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, WineReviewViewModel>()
                .ForMember(
                    x => x.CreatedOn,
                    opt => opt.MapFrom(m => m.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
