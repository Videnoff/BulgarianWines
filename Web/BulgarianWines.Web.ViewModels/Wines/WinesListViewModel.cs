namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class WinesListViewModel : PagingViewModel, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<AllWinesViewModel> Wines { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }

        public override Dictionary<string, string> GetPageQuery(int pageNumber)
        {
            var baseDictionary = base.GetPageQuery(pageNumber);

            if (this.ItemsPerPage != this.ItemsPerPageValues.FirstOrDefault())
            {
                baseDictionary.Add("ItemsPerPage", this.ItemsPerPage.ToString());
            }

            return baseDictionary;
        }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, WinesListViewModel>()
                .ForMember(x => x.CategoryName, opt =>
                    opt.MapFrom(x =>
                        x.Name));
        }
    }
}
