namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int WineId { get; set; }

        public virtual Wine Wine { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
