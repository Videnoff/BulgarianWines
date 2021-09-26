namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;

    public class WinesListViewModel
    {
        public IEnumerable<AllWinesViewModel> Wines { get; set; }

        public int PageNumber { get; set; }
    }
}
