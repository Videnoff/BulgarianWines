namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class Availability : BaseModel<int>
    {
        public Availability()
        {
            this.Wines = new HashSet<Wine>();
        }

        public string Status { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
