namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCartProduct> shoppingCartProductRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWinesService winesService;

        public ShoppingCartService(
            IRepository<ShoppingCartProduct> shoppingCartProductRepository,
            UserManager<ApplicationUser> userManager,
            IWinesService winesService)
        {
            this.shoppingCartProductRepository = shoppingCartProductRepository;
            this.userManager = userManager;
            this.winesService = winesService;
        }

        public async Task<bool> AddProductAsync(bool isUserAuthenticated, ISession session, string userId, int productId, int quantity = 1)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                var shoppingCartExists = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId) != null;

                if (shoppingCartExists)
                {
                    return false;
                }

                var productExists = this.winesService.HasProduct(productId);

                if (!productExists)
                {
                    return false;
                }

                var shoppingCart = new ShoppingCartProduct
                {
                    ShoppingCartId = shoppingCartId,
                    WineId = productId,
                    Quantity = quantity,
                };

                await this.shoppingCartProductRepository.AddAsync(shoppingCart);
                await this.shoppingCartProductRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                var shoppingCartSession = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);

                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingCartProductViewModel>();
                }

                if (shoppingCartSession.Any(x => x.ProductId == productId))
                {
                    return false;
                }

                var product = this.winesService.GetById<SingleProductViewModel>(productId);
                var shoppingCartProduct = new ShoppingCartProductViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ImageUrl = product.ImageUrl,
                    AverageRating = product.AverageRating,
                    Quantity = quantity,
                };

                shoppingCartSession.Add(shoppingCartProduct);

                session.SetObjectAsJson(GlobalConstants.SessionShoppingCartKey, shoppingCartSession);

                return true;
            }
        }

        public async Task<bool> UpdateQuantityAsync(bool isUserAuthenticated, ISession session, string userId, int productId, bool increase)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var shoppingCartId = user.ShoppingCartId;

            var shoppingCart = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId);

            if (shoppingCart == null)
            {
                return false;
            }

            var quantity = shoppingCart.Quantity;
            if (increase)
            {
                quantity++;
            }
            else
            {
                quantity = Math.Max(quantity - 1, 1);
            }

            shoppingCart.Quantity = quantity;

            this.shoppingCartProductRepository.Update(shoppingCart);
            await this.shoppingCartProductRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAllProductsAsync<T>(bool isUserAuthenticated, ISession session, string userId)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                return this.shoppingCartProductRepository
                    .AllAsNoTracking()
                    .Where(x => x.ShoppingCartId == shoppingCartId)
                    .To<T>()
                    .ToList();
            }
            else
            {
                var products = session.GetObjectFromJson<List<T>>(GlobalConstants.SessionShoppingCartKey);
                return products;
            }
        }

        public async Task<bool> DeleteProductAsync(bool isUserAuthenticated, ISession session, string userId, int productId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var shoppingCartId = user.ShoppingCartId;

            var shoppingCart = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId);

            if (shoppingCart == null)
            {
                return false;
            }

            this.shoppingCartProductRepository.Delete(shoppingCart);
            await this.shoppingCartProductRepository.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAllProductsAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var shoppingCartId = user.ShoppingCartId;

            var products = this.shoppingCartProductRepository.All()
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .ToList();

            foreach (var product in products)
            {
                this.shoppingCartProductRepository.Delete(product);
            }

            await this.shoppingCartProductRepository.SaveChangesAsync();
        }

        public async Task<int> GetProductsCountAsync(bool isUserAuthenticated, ISession session, string userId)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                return this.shoppingCartProductRepository.AllAsNoTracking()
                    .Count(x => x.ShoppingCartId == shoppingCartId);
            }
            else
            {
                var products = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                return (products == null) ? 0 : products.Count;
            }
        }

        private ShoppingCartProduct GetShoppingCartByIdAndProductId(int productId, string shoppingCartId) =>
            this.shoppingCartProductRepository.All()
                .FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.WineId == productId);
    }
}
