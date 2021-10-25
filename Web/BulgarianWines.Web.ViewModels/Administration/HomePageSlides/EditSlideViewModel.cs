namespace BulgarianWines.Web.ViewModels.Administration.HomePageSlides
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class EditSlideViewModel : CreateSlideInputViewModel, IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public IEnumerable<SlideImage> SlideImages { get; set; }
    }
}
