﻿#nullable disable

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IDSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Pages.Account;

[SecurityHeaders]
[AllowAnonymous]
public class LoginWith2faModel : PageModel
{
    private readonly SignInManager<NaturalPerson> _signInManager;
    private readonly NaturalPersonManager _userManager;
    private readonly ILogger<LoginWith2faModel> _logger;
    private readonly IIdentityServerInteractionService interactionService;
    private readonly IEventService eventService;

    public LoginWith2faModel(
        SignInManager<NaturalPerson> signInManager,
        NaturalPersonManager userManager,
        ILogger<LoginWith2faModel> logger,
        IIdentityServerInteractionService interactionService,
        IEventService eventService)
    {
        this._signInManager = signInManager;
        this._userManager = userManager;
        this._logger = logger;
        this.interactionService = interactionService;
        this.eventService = eventService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(7, ErrorMessage = "Validate_StringLength", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Two-factor code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this device")]
        public bool RememberMachine { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        _ = await this._signInManager.GetTwoFactorAuthenticationUserAsync() ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        this.ReturnUrl = returnUrl;
        this.RememberMe = rememberMe;

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        returnUrl ??= this.Url.Content("~/");
        var context = await this.interactionService.GetAuthorizationContextAsync(returnUrl);

        var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync() ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        var authenticatorCode = this.Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result = await this._signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, this.Input.RememberMachine);
        _ = await this._userManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            this._logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
            await this.eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName, clientId: context?.Client.ClientId));

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(returnUrl);
                }

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return this.Redirect(returnUrl);
            }

            // request for a local page
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else if (string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }
        else if (result.IsLockedOut)
        {
            this._logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
            return this.RedirectToPage("./Lockout");
        }
        else
        {
            this._logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
            this.ModelState.AddModelError(string.Empty, "身份验证器代码无效。");
            return this.Page();
        }
    }
}
