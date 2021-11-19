namespace BulgarianWines.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public byte Rating { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int WineId { get; set; }

        public virtual Wine Wine { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
