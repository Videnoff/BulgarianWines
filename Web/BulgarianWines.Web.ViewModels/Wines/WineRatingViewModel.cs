using System;

namespace BulgarianWines.Web.ViewModels.Wines
{
    public class WineRatingViewModel
    {
        public double AverageRating { get; set; }

        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;
    }
}
