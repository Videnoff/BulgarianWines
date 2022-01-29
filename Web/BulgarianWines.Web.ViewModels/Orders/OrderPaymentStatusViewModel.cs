namespace BulgarianWines.Web.ViewModels.Orders
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services.Mapping;

    public class OrderPaymentStatusViewModel : IMapFrom<Order>
    {
        public string Id { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
