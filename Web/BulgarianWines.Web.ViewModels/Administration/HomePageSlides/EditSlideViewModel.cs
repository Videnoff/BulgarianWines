namespace BulgarianWines.Web.ViewModels.Administration.HomePageSlides
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class EditSlideViewModel : CreateSlideInputViewModel, IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public new IFormFile Image { get; set; }
    }
}
