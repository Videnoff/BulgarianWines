namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
        }

        public string Name { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
