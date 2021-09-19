namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Variety : BaseDeletableModel<int>
    {
        public Variety()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Name { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}