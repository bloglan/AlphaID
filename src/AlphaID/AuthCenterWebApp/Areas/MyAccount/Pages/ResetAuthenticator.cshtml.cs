﻿#nullable disable

using AuthCenterWebApp;
using IDSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.MyAccount.Pages;

public class ResetAuthenticatorModel : PageModel
{
    private readonly NaturalPersonManager _userManager;
    private readonly SignInManager<NaturalPerson> _signInManager;
    private readonly ILogger<ResetAuthenticatorModel> _logger;

    public ResetAuthenticatorModel(
        NaturalPersonManager userManager,
        SignInManager<NaturalPerson> signInManager,
        ILogger<ResetAuthenticatorModel> logger)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._logger = logger;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        return user == null ? this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.") : this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        await this._userManager.SetTwoFactorEnabledAsync(user, false);
        await this._userManager.ResetAuthenticatorKeyAsync(user);
        _ = await this._userManager.GetUserIdAsync(user);
        this._logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

        await this._signInManager.RefreshSignInAsync(user);
        this.StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

        return this.RedirectToPage("./EnableAuthenticator");
    }
}