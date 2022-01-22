namespace BulgarianWines.Web.ViewModels.ShoppingCart
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Web.ViewModels.ShoppingBagAndFavorites;

    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCartProductViewModel> Products { get; set; }

        public decimal GrandTotalPrice => this.Products.Sum(x => x.TotalPrice);
    }
}
