namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Services.Messaging;
    using BulgarianWines.Web.ViewModels.Orders;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IRepository<WineOrder> wineOrdersRepository;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IWinesService winesService;
        private readonly IEmailSender emailSender;
        private readonly IRenderViewService renderViewService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IRepository<WineOrder> wineOrdersRepository,
            IShoppingCartService shoppingCartService,
            IWinesService winesService,
            IEmailSender emailSender,
            IRenderViewService renderViewService)
        {
            this.ordersRepository = ordersRepository;
            this.wineOrdersRepository = wineOrdersRepository;
            this.shoppingCartService = shoppingCartService;
            this.winesService = winesService;
            this.emailSender = emailSender;
            this.renderViewService = renderViewService;
        }

        public async Task CreateAsync<T>(T model, string userId)
        {
            await this.CancelAnyProcessingOrders(userId);

            var order = AutoMapperConfig.MapperInstance.Map<Order>(model);
            order.UserId = userId;
            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task<string> CompleteOrderAsync(string userId)
        {
            var order = this.GetProcessingOrderByUserId(userId);
            if (order == null)
            {
                return null;
            }

            var shoppingCartProducts = await this.shoppingCartService.GetAllProductsAsync<ShoppingCartProductViewModel>(true, null, userId);
            if (shoppingCartProducts == null || shoppingCartProducts.Count() == 0)
            {
                return null;
            }

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {

                if (shoppingCartProduct.Quantity >= 10)
                {
                    shoppingCartProduct.ProductPrice = shoppingCartProduct.PriceAbove10;
                }

                if (shoppingCartProduct.Quantity is < 10 and >= 5)
                {
                    shoppingCartProduct.ProductPrice = shoppingCartProduct.Price5To10;
                }

                var productOrder = new WineOrder
                {
                    Order = order,
                    WineId = shoppingCartProduct.ProductId,
                    Quantity = shoppingCartProduct.Quantity,
                    Price = shoppingCartProduct.ProductPrice,
                };

                if (!this.OrderHasProduct(order.Id, shoppingCartProduct.ProductId))
                {
                    order.Wines.Add(productOrder);
                }
            }

            order.TotalPrice = order.Wines.Sum(x => x.Quantity * x.Price);

            if (order.PaymentType is PaymentType.CashOnDelivery or PaymentType.PayNow || order.PaymentStatus == PaymentStatus.Paid)
            {
                await this.shoppingCartService.DeleteAllProductsAsync(userId);
                order.Status = OrderStatus.Unprocessed;

                // Send Email With Order Details
                var orderViewModel = this.GetById<OrderViewModel>(order.Id);
                orderViewModel.Status = order.Status;
                orderViewModel.TotalPrice = order.TotalPrice;

                var emailContent = await this.renderViewService.RenderToStringAsync("/Views/Orders/OrderRegisterEmailModel.cshtml", orderViewModel);
                await this.emailSender.SendEmailAsync("bulsing@baramail.com", "Bulsing", orderViewModel.Email, "Successfully registered order", emailContent);
            }

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return order.Id;
        }

        public async Task<bool> SetOrderStatusAsync(string id, string status)
        {
            var order = this.GetOrderById(id);

            if (order == null)
            {
                return false;
            }

            var statusResult = Enum.TryParse<OrderStatus>(status, out var statusParsed);
            if (!statusResult)
            {
                return false;
            }

            order.Status = statusParsed;

            if (statusParsed == OrderStatus.Delivered)
            {
                order.IsDelivered = true;
                order.DeliveredOn = DateTime.UtcNow;
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                order.IsDelivered = false;
                order.DeliveredOn = null;
            }

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> TakeOrdersByUserId<T>(string userId, int page, int ordersToTake) => this.ordersRepository.AllAsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>()
            .ToList();

        public IEnumerable<T> TakeOrdersByStatus<T>(OrderStatus status, int page, int ordersToTake) => this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>()
            .ToList();

        public IEnumerable<T> TakeProcessingAndUnprocessedOrders<T>(int page, int ordersToTake) => this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Status == OrderStatus.Processing || x.Status == OrderStatus.Unprocessed)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>()
            .ToList();

        public IEnumerable<T> TakeDeletedOrders<T>(int page, int ordersToTake) => this.ordersRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>()
            .ToList();

        public IEnumerable<T> GetMostBoughtProducts<T>(int productsToTake)
        {
            var productIds = this.wineOrdersRepository.AllAsNoTracking()
                .Where(x => x.Order.Status == OrderStatus.Delivered)
                .GroupBy(x => x.WineId)
                .Select(x => new { ProductId = x.Key, Total = x.Count() })
                .OrderByDescending(x => x.Total)
                .Take(productsToTake)
                .ToList();

            var products = new List<T>();

            foreach (var product in productIds)
            {
                var mappedProduct = this.winesService.GetById<T>(product.ProductId);
                products.Add(mappedProduct);
            }

            return products;
        }

        public int GetOrdersCountByUserId(string userId) => this.ordersRepository.AllAsNoTracking()
            .Count(x => x.UserId == userId);

        public int GetOrdersCountByStatus(OrderStatus status) => this.ordersRepository.AllAsNoTracking()
            .Count(x => x.Status == status);

        public int GetDeletedOrdersCount() => this.ordersRepository.AllAsNoTrackingWithDeleted()
            .Count(x => x.IsDeleted);

        public T GetById<T>(string id) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>().FirstOrDefault();

        public Order GetProcessingOrderByUserId(string userId) =>
            this.ordersRepository.All().Include(x => x.Wines)
                .FirstOrDefault(x => x.UserId == userId && x.Status == OrderStatus.Processing);

        public PaymentType GetPaymentTypeById(string id) =>
            this.ordersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id)
                .PaymentType;

        public bool UserHasOrder(string userId, string orderId) => this.ordersRepository.AllAsNoTracking()
            .Any(x => x.UserId == userId && x.Id == orderId);

        public async Task FulfillOrderById(string orderId, string stripeId)
        {
            var order = this.GetOrderById(orderId);

            order.PaymentStatus = PaymentStatus.Paid;
            order.StripeId = stripeId;

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task CancelAnyProcessingOrders(string userId)
        {
            var order = this.GetProcessingOrderByUserId(userId);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                this.ordersRepository.Update(order);
                await this.ordersRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = this.GetOrderById(id);
            if (order == null)
            {
                return false;
            }

            this.ordersRepository.Delete(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(string id)
        {
            var order = this.GetDeletedOrderById(id);
            if (order == null)
            {
                return false;
            }

            this.ordersRepository.Undelete(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        private bool OrderHasProduct(string orderId, int productId) =>
            this.ordersRepository.AllAsNoTracking()
                .Any(x => x.Id == orderId && x.Wines.Any(x => x.Id == productId));

        private Order GetOrderById(string id) =>
            this.ordersRepository.All()
                .FirstOrDefault(x => x.Id == id);

        private Order GetDeletedOrderById(string id) =>
            this.ordersRepository.AllAsNoTrackingWithDeleted()
                .Where(x => x.IsDeleted && x.Id == id)
                .FirstOrDefault();
    }
}
