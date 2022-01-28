namespace BulgarianWines.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrdersService ordersService;

        private readonly string userId;

        public OrdersController(
            IShoppingCartService shoppingCartService,
            IOrdersService ordersService,
            IHttpContextAccessor contextAccessor)
        {
            this.shoppingCartService = shoppingCartService;
            this.ordersService = ordersService;

            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        public async Task<IActionResult> Create()
        {
            var hasProduct = await this.shoppingCartService.AnyProductsAsync(this.userId);

            if (hasProduct)
            {
                this.TempData["Error"] = "Your shopping cart is empty!";
                return this.RedirectToAction("Index", "Home");
            }

            var addresses = this.addressesService.GetAll<AddressViewModel>(this.userId);
        }
    }
}