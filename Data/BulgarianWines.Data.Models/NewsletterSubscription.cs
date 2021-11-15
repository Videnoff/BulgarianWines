namespace BulgarianWines.Data.Models
{
    using System;

    using BulgarianWines.Data.Common.Models;

    public class NewsletterSubscription : BaseDeletableModel<string>
    {
        public NewsletterSubscription()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ChannelName { get; set; }

        public string ContactId { get; set; }

        public Contact Contact { get; set; }
    }
}
