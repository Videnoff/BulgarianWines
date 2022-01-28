namespace BulgarianWines.Web.ViewModels.ShoppingCart
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class ShoppingCartUpdateModel : IMapFrom<ShoppingCartProduct>
    {
        public string Id { get; set; }

        public int Quantity { get; set; }
    }
}
