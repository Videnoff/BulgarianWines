namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using System.Globalization;

    using AutoMapper;
    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class DeletedCategoryViewModel : CategoryViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, DeletedCategoryViewModel>()
                .ForMember(
                    x => x.DeletedOn,
                    d => d.MapFrom(m =>
                        m.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
