namespace BulgarianWines.Web.ViewModels.Wines
{
    public class AllWinesViewModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
