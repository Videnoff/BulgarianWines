using System.Collections.Generic;

namespace BulgarianWines.Web.ViewModels.Wines
{
    public class WinesListViewModel
    {
        public IEnumerable<AllWinesViewModel> Wines { get; set; }

        public int PageNumber { get; set; }
    }
}