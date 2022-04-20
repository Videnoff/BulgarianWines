﻿namespace BulgarianWines.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Services.Messaging;
    using BulgarianWines.Web.Infrastructure;
    using BulgarianWines.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly Services.Messaging.IEmailSender emailSender;
        private readonly ILogger<LoginModel> logger;
        private readonly IShoppingCartService shoppingCartService;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IShoppingCartService shoppingCartService)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.shoppingCartService = shoppingCartService;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email / Username")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl ??= this.Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var userName = this.Input.Email;

                if (this.IsValidEmail(this.Input.Email))
                {
                    var user = await this.userManager.FindByEmailAsync(this.Input.Email);

                    if (user != null)
                    {
                        userName = user.UserName;
                    }

                    if (user is {EmailConfirmed: false} && (await this.userManager.CheckPasswordAsync(user, this.Input.Password)))
                    {
                        this.ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                        return this.Page();
                    }
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager.PasswordSignInAsync(userName, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    var cart = this.HttpContext.Session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                    if (cart != null)
                    {
                        foreach (var product in cart)
                        {
                            var user = await this.userManager.FindByEmailAsync(this.Input.Email);
                            await this.shoppingCartService.AddProductAsync(true, this.HttpContext.Session, user.Id, product.ProductId, product.Quantity);
                        }

                        this.HttpContext.Session.Remove(GlobalConstants.SessionShoppingCartKey);
                    }

                    return this.LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return this.RedirectToPage("./LoginWith2fa", new
                    {
                        ReturnUrl = returnUrl,
                        RememberMe = this.Input.RememberMe,
                    });
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
