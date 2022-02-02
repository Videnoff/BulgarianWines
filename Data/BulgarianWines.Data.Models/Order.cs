namespace BulgarianWines.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BulgarianWines.Data.Common.Models;
    using BulgarianWines.Data.Models.Enums;

    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Wines = new HashSet<WineOrder>();
            this.IsDelivered = false;
            this.PaymentStatus = PaymentStatus.Unpaid;
            this.Status = OrderStatus.Processing;
        }

        [Required]
        public string UserFirstName { get; set; }

        [Required]
        public string UserLastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public PaymentType PaymentType { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsDelivered { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public virtual ICollection<WineOrder> Wines { get; set; }
    }
}
