namespace BulgarianWines.Web.Areas.Identity.ViewModels
{
    using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

    public class RegisterViewModel : Pages.Account.RegisterModel.InputModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }
    }
}
