namespace BulgarianWines.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Stripe.Checkout;

    [Authorize]
    [Route("/api/checkout")]
    [ApiController]
    public class CheckoutController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly string userId;
        private readonly string domain;

        public CheckoutController(
            IOrdersService ordersService,
            IHttpContextAccessor contextAccessor)
        {
            this.ordersService = ordersService;
            this.domain = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}";
            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Create()
        {
            var orderId = this.ordersService.GetProcessingOrderByUserId(this.userId).Id;
            var order = this.ordersService.GetById<OrderViewModel>(orderId);

            var items = new List<SessionLineItemOptions>();

            foreach (var wine in order.Wines)
            {
                items.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(wine.Price * 100),
                        Currency = "sgd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = wine.WineName,
                            Images = new List<string>
                            {
                                wine.ImageUrl,
                            },
                        },
                    },

                    Quantity = wine.Quantity,
                });
            }

            // Add shipping price
            //items.Add(new SessionLineItemOptions()
            //{
            //    PriceData = new SessionLineItemPriceDataOptions()
            //    {
            //        UnitAmount = (long?)(order.DeliveryPrice * 100),
            //        Currency = "sgd",
            //        ProductData = new SessionLineItemPriceDataProductDataOptions()
            //        {
            //            Name = "Shipping",
            //        },
            //    },
            //    Quantity = 1,
            //});

            var options = new SessionCreateOptions()
            {
                PaymentMethodTypes = new List<string>()
                {
                    "card",
                },
                LineItems = items,
                Mode = "payment",
                SuccessUrl = this.domain + "/Orders/Complete",
                CancelUrl = this.domain + "/Orders/Create",
                Metadata = new Dictionary<string, string>()
                {
                    {
                        "order_id",
                        orderId
                    },
                },
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return this.Json(new
            {
                id = session.Id,
            });
        }
    }
}
