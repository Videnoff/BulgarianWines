namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class SlideImage : BaseDeletableModel<string>
    {
        public SlideImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int HomePageSlideId { get; set; }

        public virtual HomePageSlide HomePageSlide { get; set; }

        public string ImageUrl { get; set; }
    }
}
