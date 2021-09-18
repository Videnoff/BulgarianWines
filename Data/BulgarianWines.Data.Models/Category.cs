namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Name { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
