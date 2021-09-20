namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateWineInputModel
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

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VolumesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> HarvestsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> VarietyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> OriginsItems { get; set; }
    }
}
