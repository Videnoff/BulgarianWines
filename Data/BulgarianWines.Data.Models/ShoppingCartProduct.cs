namespace BulgarianWines.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class ShoppingCartProduct : BaseDeletableModel<string>
    {
        public ShoppingCartProduct()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        public int WineId { get; set; }

        public virtual Wine Wine { get; set; }

        public int Quantity { get; set; }
    }
}
