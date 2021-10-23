namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CategoryImages = new HashSet<CategoryImage>();
            this.Wines = new HashSet<Wine>();
        }

        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }

        public virtual ICollection<CategoryImage> CategoryImages { get; set; }
    }
}
