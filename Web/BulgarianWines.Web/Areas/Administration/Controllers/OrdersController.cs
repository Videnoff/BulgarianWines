using BulgarianWines.Data.Models.Enums;
using BulgarianWines.Services.Data;
using BulgarianWines.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    public class OrdersController : AdministrationController
    {
        private const int ItemsPerPage = 8;
        private const string AreaName = "Administration";
        private const string ControllerName = "Orders";

        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IActionResult Unprocessed(int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return this.Unprocessed();
            }

            var allOrders =
                this.ordersService.TakeProcessingAndUnprocessedOrders<OrderSummaryViewModel>(pageNumber, ItemsPerPage);
            var unprocessedOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Unprocessed);
            var processingOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Processing);

            var viewModel = new OrderListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = pageNumber,
                Orders = allOrders,
                Area = AreaName,
                Controller = ControllerName,
                Action = nameof(this.Unprocessed),
            };

            return this.View(viewModel);
        }

        public IActionResult Processed(int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return this.Processed();
            }

            var processedOrders = this.ordersService.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Processed, pageNumber, ItemsPerPage);
            var processedOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Processed);

            var viewModel = new OrderListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = pageNumber,
                Orders = processedOrders,
                Area = AreaName,
                Controller = ControllerName,
                Action = nameof(this.Processed),
            };

            return this.View(viewModel);
        }

        public IActionResult Delivered(int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return this.Delivered();
            }

            var deliveredOrders =
                this.ordersService.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Delivered, pageNumber,
                    ItemsPerPage);
            var deliveredOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Delivered);

            var viewModel = new OrderListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = pageNumber,
                Orders = deliveredOrders,
                Area = AreaName,
                Controller = ControllerName,
                Action = nameof(this.Delivered),
            };

            return this.View(viewModel);
        }
    }
}