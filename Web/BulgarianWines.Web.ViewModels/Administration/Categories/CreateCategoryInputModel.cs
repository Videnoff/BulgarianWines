namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class CreateCategoryInputModel : IMapTo<Category>
    {
        [Display(Name = "Add Image")]
        [Image]
        public IEnumerable<IFormFile> UploadedImages { get; set; }

        public string Name { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        public string Description { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Created on")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime? ModifiedOn { get; set; }
    }
}
