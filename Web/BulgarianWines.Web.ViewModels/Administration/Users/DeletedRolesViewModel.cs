namespace BulgarianWines.Web.ViewModels.Administration.Users
{
    using System.Globalization;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class DeletedRolesViewModel : ApplicationRole, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationRole, DeletedRolesViewModel>()
                .ForMember(
                    x => x.DeletedOn,
                    d => d.MapFrom(m =>
                        m.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
