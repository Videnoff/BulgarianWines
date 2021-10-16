namespace BulgarianWines.Web.ViewModels.Administration.Wines
{
    using System.Collections.Generic;

    using BulgarianWines.Web.ViewModels.Wines;

    public class WinesViewModel : PagingViewModel
    {
        public IEnumerable<ProductViewModel> Wines { get; set; }
    }
}