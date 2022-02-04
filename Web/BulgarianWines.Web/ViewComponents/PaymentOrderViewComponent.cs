namespace BulgarianWines.Web.ViewComponents
{
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class PaymentOrderViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;

        public PaymentOrderViewComponent(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke(string orderId)
        {
            var paymentType = this.ordersService.GetPaymentTypeById(orderId);

            var viewModel = new PaymentViewModel
            {
                PaymentType = paymentType,
            };

            return this.View(viewModel);
        }
    }
}
