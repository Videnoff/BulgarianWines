﻿namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IShoppingCartService
    {
        public Task<bool> AddProductAsync(bool isUserAuthenticated, ISession session, string userId, int productId, int quantity = 1);

        public Task<bool> UpdateQuantityAsync(bool isUserAuthenticated, ISession session, string userId, int productId, bool increase);

        public Task<IEnumerable<T>> GetAllProductsAsync<T>(bool isUserAuthenticated, ISession session, string userId);

        public Task<bool> DeleteProductAsync(bool isUserAuthenticated, ISession session, string userId, int productId);

        public Task DeleteAllProductsAsync(string userId);

        public Task<bool> AnyProductsAsync(string userId);

        public Task<int> GetProductsCountAsync(bool isUserAuthenticated, ISession session, string userId);
    }
}
