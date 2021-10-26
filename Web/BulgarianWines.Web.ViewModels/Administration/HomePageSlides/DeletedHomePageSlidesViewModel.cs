namespace BulgarianWines.Web.ViewModels.Administration.HomePageSlides
{
    using System.Globalization;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels.HomePage;

    public class DeletedHomePageSlidesViewModel : HomePageViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HomePageSlide, DeletedHomePageSlidesViewModel>()
                .ForMember(
                    x => x.DeletedOn,
                    d => d.MapFrom(m =>
                        m.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
