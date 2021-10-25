namespace BulgarianWines.Web.ViewModels.Administration.HomePageSlides
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class CreateSlideInputViewModel : IMapTo<HomePageSlide>
    {
        [Display(Name = "Add Images")]
        [ImageAttribute]
        public IEnumerable<IFormFile> UploadedImages { get; set; }

        [Required]
        public string Description { get; set; }

        public string LinkUrl { get; set; }
    }
}
