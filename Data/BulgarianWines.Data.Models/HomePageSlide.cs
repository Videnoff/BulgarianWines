﻿namespace BulgarianWines.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class HomePageSlide : BaseModel<int>
    {
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string LinkUrl { get; set; }

        public int Position { get; set; }
    }
}
