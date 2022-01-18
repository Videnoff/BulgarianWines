namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BulgarianWines.Web.ViewModels.Wines;

    public class CategoryProductsViewModel : PagingViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<string> SortingValues { get; set; }

        public string Sorting { get; set; }

        public IEnumerable<int> ItemsPerPageValues { get; set; }

        public double AverageRating { get; set; }

        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;

        public bool ShowAverageRating { get; set; } = true;

        // public override Dictionary<string, string> GetPageQuery(int pageNumber)
        // {
        //    var baseDictionary = base.GetPageQuery(pageNumber);
        //    baseDictionary.Add("SubcategoryId", this.Id.ToString());

        // if (this.ItemsPerPage != this.ItemsPerPageValues.FirstOrDefault())
        //    {
        //        baseDictionary.Add("ItemsPerPage", this.ItemsPerPage.ToString());
        //    }

        // if (this.Sorting.ToLower() != this.SortingValues.FirstOrDefault().ToLower())
        //    {
        //        baseDictionary.Add("Sorting", this.Sorting.ToString());
        //    }

        // return baseDictionary;
        // }
    }
}
