namespace BulgarianWines.Web.ViewModels.Wines
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

        [Display(Name = "Add Images")]
        [ImageAttribute]
        public IEnumerable<IFormFile> UploadedImages { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VolumesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> HarvestsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VarietyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> OriginsItems { get; set; }
    }
}
