namespace BulgarianWines.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        public async Task CreateAsync<T>(T model, string userId)
        {
            await this.CancelAnyProcessingOrders(userId);

            var order = AutoMapperConfig.MapperInstance.Map<Order>(model);
            order.UserId = userId;
            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();
        }

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

        private bool OrderHasProduct(string orderId, int productId) =>
            this.ordersRepository.AllAsNoTracking()
                .Any(x => x.Id == orderId && x.Wines.Any(x => x.Id == productId));
    }
}
