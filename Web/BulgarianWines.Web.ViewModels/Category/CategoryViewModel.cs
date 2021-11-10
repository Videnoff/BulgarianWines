namespace BulgarianWines.Web.ViewModels.Category
{
    using BulgarianWines.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Data.Models.Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
