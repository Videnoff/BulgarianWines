namespace BulgarianWines.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BulgarianWines.Data.Common.Models;

    public class Order : BaseDeletableModel<string>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }

        public bool IsDelivered { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public virtual ICollection<WineOrder> ProductOrders { get; set; }
    }
}
