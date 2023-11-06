﻿#nullable disable

using IDSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.Settings.Pages.Authentication;

public class Disable2FaModel : PageModel
{
    private readonly NaturalPersonManager userManager;
    private readonly ILogger<Disable2FaModel> logger;

    public Disable2FaModel(
        NaturalPersonManager userManager,
        ILogger<Disable2FaModel> logger)
    {
        this.userManager = userManager;
        this.logger = logger;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await this.userManager.GetUserAsync(this.User);
        return user == null
            ? this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.")
            : !await this.userManager.GetTwoFactorEnabledAsync(user)
            ? throw new InvalidOperationException("Cannot disable 2FA for user as it's not currently enabled.")
            : (IActionResult)this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await this.userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
        }

        var disable2FaResult = await this.userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2FaResult.Succeeded)
        {
            throw new InvalidOperationException("Unexpected error occurred disabling 2FA.");
        }

        this.logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", this.userManager.GetUserId(this.User));
        this.StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
        return this.RedirectToPage("./TwoFactorAuthentication");
    }
}
