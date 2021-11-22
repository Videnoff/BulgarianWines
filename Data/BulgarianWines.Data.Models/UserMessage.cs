namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class UserMessage : BaseDeletableModel<string>
    {
        public UserMessage()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsRead = false;
        }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }
    }
}
