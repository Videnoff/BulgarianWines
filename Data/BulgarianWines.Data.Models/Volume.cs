namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Volume : BaseDeletableModel<int>
    {
        public Volume()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Quantity { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
