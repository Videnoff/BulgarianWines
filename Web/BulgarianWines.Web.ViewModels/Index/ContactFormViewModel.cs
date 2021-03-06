namespace BulgarianWines.Web.ViewModels.Index
{
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;

    public class ContactFormViewModel : IMapTo<UserMessage>
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        [GoogleReCaptcha]
        public string RecaptchaValue { get; set; }
    }
}
