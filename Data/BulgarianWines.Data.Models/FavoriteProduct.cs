namespace BulgarianWines.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class FavoriteProduct : BaseDeletableModel<string>
    {
        public FavoriteProduct()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int WineId { get; set; }

        public virtual Wine Wine { get; set; }
    }
}
