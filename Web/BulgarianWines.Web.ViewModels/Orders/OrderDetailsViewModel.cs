using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AutoMapper;
using BulgarianWines.Common;
using BulgarianWines.Data.Models;
using BulgarianWines.Data.Models.Enums;
using BulgarianWines.Services.Mapping;

namespace BulgarianWines.Web.ViewModels.Orders
{
    public class OrderDetailsViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        [Display(Name = "Order Id")]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string UserFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string UserLastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal DeliveryPrice { get; set; }

        [Display(Name = "Total Price (USD)")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }

        [Display(Name = "Payment Type")]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        public bool IsDelivered { get; set; }

        [Display(Name = "Delivered On")]
        public string DeliveredOn { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Delivery Type")]
        public DeliveryType DeliveryType { get; set; }

        [Display(Name = "Stripe Payment Id")]
        public string StripeId { get; set; }

        public IEnumerable<OrderWinesViewModel> Wines { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderDetailsViewModel>()
            .ForMember(
                source => source.Address,
                destination => destination.MapFrom(member => $"{member.Address.Street} {member.Address.City.Name}, {member.Address.City.PostCode}"))
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
            .ForMember(
                source => source.DeliveredOn,
                destination => destination.MapFrom(member => (member.DeliveredOn == null) ? null : member.DeliveredOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}