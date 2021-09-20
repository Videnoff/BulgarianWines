namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Origin : BaseDeletableModel<int>
    {
        public Origin()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Name { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
