namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class WinesListViewModel : PagingViewModel, IHaveCustomMappings
    {
        public string CategoryName { get; set; }

        public IEnumerable<AllWinesViewModel> Wines { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, WinesListViewModel>()
                .ForMember(x => x.CategoryName, opt =>
                    opt.MapFrom(x =>
                        x.Name));
        }
    }
}
