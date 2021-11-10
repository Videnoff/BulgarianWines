namespace BulgarianWines.Web.ViewModels.Search
{
    using System.Collections.Generic;

    using BulgarianWines.Web.ViewModels.Category;

    public class SearchInputViewModel
    {
        public string SearchTerm { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
