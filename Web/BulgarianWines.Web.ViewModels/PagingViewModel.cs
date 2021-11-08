﻿namespace BulgarianWines.Web.ViewModels
{
    using System;

    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.WinesCount / this.ItemsPerPage);

        public int FirstPage => 1;

        public int WinesCount { get; set; }

        public int ItemsPerPage { get; set; }

        public string Area { get; set; } = string.Empty;

        public string Controller { get; set; }

        public string Action { get; set; }
    }
}
