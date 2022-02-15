namespace BulgarianWines.Web.ViewModels.Administration.Wines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class CreateWineInputModel : IMapTo<Wine>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1, (double)int.MaxValue)]
        [Display(Name = "Price 1 to 5 (SGD)")]
        public decimal Price { get; set; }

        [Range(1, (double)int.MaxValue)]
        [Display(Name = "Price 5 to 10 (SGD)")]
        public decimal Price5To10 { get; set; }

        [Range(1, (double)int.MaxValue)]
        [Display(Name = "Price above 10 (SGD)")]
        public decimal PriceAbove10 { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Select Origin")]
        public int OriginId { get; set; }

        [Required]
        [Display(Name = "Select Variety")]
        public int VarietyId { get; set; }

        [Required]
        [Display(Name = "Select Harvest")]
        public int HarvestId { get; set; }

        [Required]
        [Display(Name = "Select Volume")]
        public int VolumeId { get; set; }

        [Required]
        [Display(Name = "Select Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        [Required]
        [Display(Name = "Select Availability")]
        public int AvailabilityId { get; set; }

        [Display(Name = "Add Images")]
        [Image]
        public IEnumerable<IFormFile> UploadedImages { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Created on")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VolumesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> HarvestsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VarietyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> OriginsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AvailabilityItems { get; set; }
    }
}
