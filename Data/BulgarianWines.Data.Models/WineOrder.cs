namespace BulgarianWines.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class WineOrder : BaseDeletableModel<int>
    {
        [Required]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public string WineId { get; set; }

        public virtual Wine Wine { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
