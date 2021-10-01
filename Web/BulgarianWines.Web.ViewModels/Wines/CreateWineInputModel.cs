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
        public int OriginId { get; set; }

        [Required]
        public int VarietyId { get; set; }

        [Required]
        public int HarvestId { get; set; }

        [Required]
        public int VolumeId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Add Images")]
        [ImageAttribute]
        public IEnumerable<IFormFile> UploadedImages { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VolumesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> HarvestsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VarietyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> OriginsItems { get; set; }
    }
}
