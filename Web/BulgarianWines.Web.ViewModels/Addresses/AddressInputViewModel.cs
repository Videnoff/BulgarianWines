namespace BulgarianWines.Web.ViewModels.Addresses
{
    using System.ComponentModel.DataAnnotations;

    public class AddressInputViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "The street must be at least 5 characters long.")]
        [MaxLength(50, ErrorMessage = "The street can be maximum 50 characters long.")]
        public string Street { get; set; }

        public string Description { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The city name must be at least 3 characters long.")]
        [MaxLength(30, ErrorMessage = "The city name can be maximum 30 characters long.")]
        public string City { get; set; }

        public string PostCode { get; set; }

        public string UserId { get; set; }
    }
}
