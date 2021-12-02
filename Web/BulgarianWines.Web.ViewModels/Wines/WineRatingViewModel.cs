namespace BulgarianWines.Web.ViewModels.Wines
{
    using System;

    public class WineRatingViewModel
    {
        public double AverageRating { get; set; }

        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;

        public bool ShowAverageRating { get; set; } = true;
    }
}
