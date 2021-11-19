namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class WineReviewInputModel : IMapTo<Review>
    {
        [Range(1, 5)]
        public byte Rating { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long")]
        [MaxLength(30, ErrorMessage = "The name must be maximum 30 characters long")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The content must be at least 3 characters long.")]
        [MaxLength(1000, ErrorMessage = "The content can be maximum 1000 characters long.")]
        public string Content { get; set; }

        public string UserId { get; set; }

        public int WineId { get; set; }
    }
}
