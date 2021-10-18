namespace BulgarianWines.Web.ViewModels.HomePage
{
    using System.Collections.Generic;

    using BulgarianWines.Web.ViewModels.Wines;

    public class IndexViewModel
    {
        public IEnumerable<ProductSidebarViewModel> MostBoughtProducts { get; set; }

        public IEnumerable<ProductViewModel> NewestProducts { get; set; }

        public IEnumerable<ProductSidebarViewModel> TopRatedProducts { get; set; }

        public IEnumerable<HomePageViewModel> Slides { get; set; }
    }
}
