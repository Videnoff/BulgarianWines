namespace BulgarianWines.Web.ViewModels.Category
{
    using BulgarianWines.Services.Mapping;

    public class AllCategoriesViewModel : IMapFrom<Data.Models.Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string ImageUrl { get; set; }
    }
}
