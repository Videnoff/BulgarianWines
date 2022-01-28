using BulgarianWines.Data.Models;
using BulgarianWines.Services.Mapping;

namespace BulgarianWines.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartUpdateModel : IMapFrom<ShoppingCartProduct>
    {
        public string Id { get; set; }

        public int Quantity { get; set; }
    }
}
