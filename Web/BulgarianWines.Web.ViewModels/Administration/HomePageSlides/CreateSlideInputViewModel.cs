namespace BulgarianWines.Web.ViewModels.Administration.HomePageSlides
{
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class CreateSlideInputViewModel : IMapTo<HomePageSlide>
    {
        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string LinkUrl { get; set; }
    }
}
