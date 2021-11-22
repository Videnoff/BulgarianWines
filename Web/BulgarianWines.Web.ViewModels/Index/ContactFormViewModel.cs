namespace BulgarianWines.Web.ViewModels.Index
{
    using System.ComponentModel.DataAnnotations;

    public class ContactFormViewModel
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
