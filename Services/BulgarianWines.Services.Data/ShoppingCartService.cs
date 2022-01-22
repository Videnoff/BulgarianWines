using BulgarianWines.Web.ViewModels.ShoppingBagAndFavorites;

namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure;
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
                var shoppingBagId = user.ShoppingCartId;

                var shoppingBagExists = this.GetShoppingCartByIdAndProductId(productId, shoppingBagId) != null;

                if (shoppingBagExists)
                {
                    return false;
                }

                var productExists = this.winesService.HasProduct(productId);

                if (!productExists)
                {
                    return false;
                }

                var newShoppingBag = new ShoppingCartProduct
                {
                    ShoppingCartId = shoppingBagId,
                    WineId = productId,
                    Quantity = quantity,
                };

                await this.shoppingCartProductRepository.AddAsync(newShoppingBag);
                await this.shoppingCartProductRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                var shoppingCartSession = session.GetObjectFromJson<List<ShoppingBagProductViewModel>>(GlobalConstants.SessionShoppingCartKey);

                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingBagProductViewModel>();
                }

                if (shoppingCartSession.Any(x => x.ProductId == productId))
                {
                    return false;
                }

                var product = this.winesService.GetById<SingleProductViewModel>(productId);
                var shoppingCartProduct = new ShoppingBagProductViewModel
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

        public Task<bool> UpdateQuantityAsync(bool isUserAuthenticated, ISession session, string userId, int productId, bool increase)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllProducts<T>(bool isUserAuthenticated, ISession session, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(bool isUserAuthenticated, ISession session, string userId, int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAllProductsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasAnyProducts(string userId)
        {
            throw new System.NotImplementedException();
        }

        private ShoppingCartProduct GetShoppingCartByIdAndProductId(int productId, string shoppingCartId) =>
            this.shoppingCartProductRepository.All()
                .FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.WineId == productId);
    }
}