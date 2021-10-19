namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;
    using Microsoft.AspNetCore.Http;

    public class HomePageSlide : BaseDeletableModel<int>
    {
        public HomePageSlide()
        {
            this.SlideImages = new HashSet<SlideImage>();
        }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string LinkUrl { get; set; }

        public int Position { get; set; }

        public virtual ICollection<SlideImage> SlideImages { get; set; }
    }
}
