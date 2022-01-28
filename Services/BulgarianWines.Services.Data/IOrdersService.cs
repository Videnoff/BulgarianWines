namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;

    public interface IOrdersService
    {
        public Task CreateAsync<T>(T model, string userId);

        public T GetById<T>(string id);

        public Order GetProcessingOrderByUserId(string userId);

        public PaymentType GetPaymentTypeById(string id);

        public Task CancelAnyProcessingOrders(string userId);
    }
}
