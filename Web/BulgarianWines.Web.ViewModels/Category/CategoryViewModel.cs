namespace BulgarianWines.Web.ViewModels.Category
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
