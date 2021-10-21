namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class UserImage : BaseDeletableModel<string>
    {
        public UserImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ImageUrl { get; set; }
    }
}
