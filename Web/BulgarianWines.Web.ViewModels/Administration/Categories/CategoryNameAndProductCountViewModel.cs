namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class CategoryNameAndProductCountViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int WinesCount { get; set; }
    }
}