namespace BulgarianWines.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels.Addresses;

    public class CreateOrderInputModel : IMapTo<Order>
    {
        [Required]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 100 characters long.")]
        [Display(Name = "Full Name")]
        public string UserFullName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 20 characters long.")]
        [Display(Name = "First Name")]
        public string UserFirstName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 20 characters long.")]
        [Display(Name = "Last Name")]
        public string UserLastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[+]?[0-9]+$", ErrorMessage = "The phone can have only numbers and can begin with a plus symbol.")]
        public string Phone { get; set; }

        [Required]
        public DeliveryType DeliveryType { get; set; }

        [Required]
        public string AddressId { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
