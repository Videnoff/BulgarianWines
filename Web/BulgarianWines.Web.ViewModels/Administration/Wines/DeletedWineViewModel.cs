using BulgarianWines.Web.ViewModels.Wines;

namespace BulgarianWines.Web.ViewModels.Administration.Wines
{
    using System.Globalization;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class DeletedWineViewModel : ProductViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, DeletedWineViewModel>()
                .ForMember(
                    x => x.DeletedOn,
                    d => d.MapFrom(m =>
                        m.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
