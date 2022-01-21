using System.Collections.Generic;
using System.Linq;

namespace BulgarianWines.Web.ViewModels.ShoppingBagAndFavorites
{
    public class ShoppingBagViewModel
    {
        public IEnumerable<ShoppingBagProductViewModel> Products { get; set; }

        //public decimal GrandTotalPrice => this.Products.Sum(x => x.TotalPrice);
    }
}
