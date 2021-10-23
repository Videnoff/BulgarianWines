namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class CategoryImage : BaseDeletableModel<string>
    {
        public CategoryImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
