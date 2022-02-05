namespace BulgarianWines.Web.ViewComponents
{
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc;

    public class OrderDetailsViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;
        private readonly IShortTextService shortTextService;

        public OrderDetailsViewComponent(
            IOrdersService ordersService,
            IShortTextService shortTextService)
        {
            this.ordersService = ordersService;
            this.shortTextService = shortTextService;
        }

        public IViewComponentResult Invoke(string orderId)
        {
            var order = this.ordersService.GetById<OrderViewModel>(orderId);

            foreach (var product in order.Wines)
            {
                product.WineName = this.shortTextService.ShortText(product.WineName, 30);
            }

            return this.View(order);
        }
    }
}
