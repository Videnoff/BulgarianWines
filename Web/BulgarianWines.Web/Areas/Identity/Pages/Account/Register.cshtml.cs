namespace BulgarianWines.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data;
    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Services.Messaging;
    using BulgarianWines.Web.Areas.Identity.ViewModels;
    using BulgarianWines.Web.Infrastructure;
    using BulgarianWines.Web.Infrastructure.ValidationAttributes;
    using BulgarianWines.Web.ViewModels.Administration.Users;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string AzureContainerName = "publicimages";

        private readonly IImagesService imagesService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Services.Messaging.IEmailSender emailSender;
        private readonly ILogger<RegisterModel> logger;
        private readonly IRenderViewService renderViewService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ApplicationDbContext dbContext;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            Services.Messaging.IEmailSender emailSender,
            IImagesService imagesService,
            IRenderViewService renderViewService,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IShoppingCartService shoppingCartService,
            ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.imagesService = imagesService;
            this.renderViewService = renderViewService;
            this.usersRepository = usersRepository;
            this.shoppingCartService = shoppingCartService;
            this.dbContext = dbContext;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]

            // [Remote(action: "IsEmailInUse", controller: "Users", areaName: "Administration")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // [Display(Name = "Select Profile Image")]
            // [ImageAttribute]
            // public IFormFile ImagePath { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file, string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                MailAddress address = new MailAddress(this.Input.Email);
                var userName = address.User;

                // await this.imagesService.UploadAzureBlobImageAsync(file, AzureContainerName);

                var shoppingCart = new ShoppingCart();

                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    ShoppingCart = shoppingCart,

                    // ImageUrl = file.FileName,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                shoppingCart.User = user;

                await this.dbContext.SaveChangesAsync();
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code = code,
                            returnUrl = returnUrl,
                        },
                        protocol: this.Request.Scheme);

                    //var viewModel = this.GetById<RegisterViewModel>(user.Id);

                    //var emailContent =
                    //    await this.renderViewService.RenderToStringAsync(
                    //        "Areas/Administration/Views/Users/RegisterEmailConfirmation.cshtml", viewModel);

                    await this.emailSender.SendEmailAsync(
                        "bulsing@baramail.com",
                        "Bulsing",
                        this.Input.Email,
                        "Confirm your email",
                        /*null*/ $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount || this.userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (this.signInManager.IsSignedIn(this.User) && (this.User.IsInRole(GlobalConstants.AdministratorRoleName) || this.User.IsInRole(GlobalConstants.SuperAdministratorRoleName)))
                        {
                            return this.RedirectToAction("ListUsers", "Users", new { area = "Administration" });
                        }

                        await this.signInManager.SignInAsync(user, isPersistent: false);

                        var cart = this.HttpContext.Session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(
                            GlobalConstants.SessionShoppingCartKey);

                        if (cart != null)
                        {
                            foreach (var product in cart)
                            {
                                await this.shoppingCartService.AddProductAsync(true, this.HttpContext.Session, user.Id, product.ProductId, product.Quantity);
                            }

                            this.HttpContext.Session.Remove(GlobalConstants.SessionShoppingCartKey);
                        }

                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public T GetById<T>(string id) =>
            this.usersRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
    }
}
