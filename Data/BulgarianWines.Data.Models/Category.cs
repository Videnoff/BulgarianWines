﻿namespace BulgarianWines.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Wines = new HashSet<Wine>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
    }
}
