using System.Collections.Generic;

namespace BulgarianWines.Data.Models
{
    using BulgarianWines.Data.Common.Models;

    public class Wine : BaseDeletableModel<int>
    {
        public Wine()
        {
            this.Images = new HashSet<Image>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}