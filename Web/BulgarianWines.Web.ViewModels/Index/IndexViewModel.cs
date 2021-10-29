namespace BulgarianWines.Web.ViewModels.Index
{
    using System.Collections.Generic;

    using BulgarianWines.Web.ViewModels.Administration.Categories;
    using BulgarianWines.Web.ViewModels.HomePage;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Http;

    public class IndexViewModel
    {
        public IEnumerable<ProductSidebarViewModel> MostBoughtProducts { get; set; }

        public IEnumerable<ProductViewModel> NewestProducts { get; set; }

        public IEnumerable<ProductSidebarViewModel> TopRatedProducts { get; set; }

        public IEnumerable<CategoryViewModel> AllCategories { get; set; }

        public IEnumerable<HomePageViewModel> Slides { get; set; }

        public IFormFile LogoImage { get; set; }
    }
}
