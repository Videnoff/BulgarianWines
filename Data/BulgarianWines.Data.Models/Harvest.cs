namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Harvest : BaseDeletableModel<int>
    {
        public Harvest()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Year { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
