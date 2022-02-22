namespace BulgarianWines.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Addresses;
    using BulgarianWines.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrdersService ordersService;
        private readonly IAddressesService addressesService;
        private readonly IShortTextService shortTextService;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly string userId;

        public OrdersController(
            IShoppingCartService shoppingCartService,
            IOrdersService ordersService,
            IHttpContextAccessor contextAccessor,
            IAddressesService addressesService,
            IShortTextService shortTextService,
            UserManager<ApplicationUser> userManager)
        {
            this.shoppingCartService = shoppingCartService;
            this.ordersService = ordersService;
            this.addressesService = addressesService;
            this.shortTextService = shortTextService;
            this.userManager = userManager;

            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Create()
        {
            var hasProduct = await this.shoppingCartService.AnyProductsAsync(this.userId);

            if (!hasProduct)
            {
                this.TempData["Error"] = "Your shopping cart is empty!";
                return this.RedirectToAction("Index", "Home");
            }

            var addresses = this.addressesService.GetAll<AddressViewModel>(this.userId);

            foreach (var address in addresses)
            {
                address.Description = this.shortTextService.ShortText(address.Description, 30);
            }

            var userName = this.User.Identity.Name;
            var user = await this.userManager.FindByNameAsync(userName);
            var email = await this.userManager.GetEmailAsync(user);

            var model = new CreateOrderInputModel
            {
                Addresses = addresses,
                Email = email,
            };

            await this.ordersService.CancelAnyProcessingOrders(this.userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var hasProducts = await this.shoppingCartService.AnyProductsAsync(this.userId);
                if (!hasProducts)
                {
                    this.TempData["Error"] = "Your shopping cart is empty";
                    return this.RedirectToAction("Index", "Home");
                }

                var addresses = this.addressesService.GetAll<AddressViewModel>(this.userId);

                foreach (var address in addresses)
                {
                    address.Description = this.shortTextService.ShortText(address.Description, 30);
                }

                var email = this.User.Identity.Name;

                model.Addresses = addresses;
                model.Email = email;

                return this.View(model);
            }

            await this.ordersService.CreateAsync<CreateOrderInputModel>(model, this.userId);

            return this.RedirectToAction(nameof(this.Complete));
        }

        public async Task<IActionResult> Complete()
        {
            var hasProducts = await this.shoppingCartService.AnyProductsAsync(this.userId);

            if (!hasProducts)
            {
                this.TempData["Error"] = "Your shopping cart is empty";
                return this.RedirectToAction("Index", "Home");
            }

            var orderId = await this.ordersService.CompleteOrderAsync(this.userId);

            if (this.ordersService.GetPaymentTypeById(orderId) == PaymentType.CashOnDelivery)
            {
                this.TempData["Alert"] = "Successfully registered order.";
            }

            var order = this.ordersService.GetById<OrderPaymentStatusViewModel>(orderId);

            return this.View(order);
        }

        [HttpGet("/Orders/History/{pageNumber?}")]
        public IActionResult History(int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return this.History();
            }

            var itemsPerPage = 6;
            var orders = this.ordersService.TakeOrdersByUserId<OrderSummaryViewModel>(this.userId, pageNumber, itemsPerPage);
            var ordersCount = this.ordersService.GetOrdersCountByUserId(this.userId);

            var viewModel = new OrderListViewModel
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Orders = orders,
                Area = string.Empty,
                Controller = "Orders",
                Action = nameof(this.History),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(string id)
        {
            if (!this.ordersService.UserHasOrder(this.userId, id))
            {
                this.TempData["Error"] = "Order not found.";
                return this.RedirectToAction("Index", "Home");
            }

            this.ViewData["OrderId"] = id;
            return this.View();
        }
    }
}
