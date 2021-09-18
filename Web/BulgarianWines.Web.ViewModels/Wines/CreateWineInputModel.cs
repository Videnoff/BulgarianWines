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
        public string Harvest { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Volume { get; set; }

        [Required]
        public string Variety { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
