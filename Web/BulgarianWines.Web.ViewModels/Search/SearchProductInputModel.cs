namespace BulgarianWines.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Web.ViewModels.Wines;

    public class SearchProductInputModel : PagingViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<string> SortingValues { get; set; }

        public string Sorting { get; set; }

        public string SearchTerm { get; set; }

        public int? CategoryId { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }
    }
}
