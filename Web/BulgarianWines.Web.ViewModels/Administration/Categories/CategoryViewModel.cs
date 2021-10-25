namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        [Display(Name = "Category Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Created on")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<CategoryImage> CategoryImages { get; set; }
    }
}
