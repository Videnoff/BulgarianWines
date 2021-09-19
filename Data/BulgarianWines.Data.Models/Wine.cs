namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Wine : BaseDeletableModel<int>
    {
        public Wine()
        {
            this.Images = new HashSet<Image>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Harvest { get; set; }

        public string Origin { get; set; }

        public string Variety { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int VolumeId { get; set; }

        public virtual Volume Volume { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
