namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;

    public class WinesListViewModel : PagingViewModel
    {
        public IEnumerable<AllWinesViewModel> Wines { get; set; }
    }
}
