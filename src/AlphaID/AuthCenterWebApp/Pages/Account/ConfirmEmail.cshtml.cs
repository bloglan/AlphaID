﻿#nullable disable

using IdSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AuthCenterWebApp.Pages.Account;

[SecurityHeaders]
[AllowAnonymous]
public class ConfirmEmailModel : PageModel
{
    private readonly NaturalPersonManager userManager;

    public ConfirmEmailModel(NaturalPersonManager userManager)
    {
        this.userManager = userManager;
    }

    [TempData]
    public string StatusMessage { get; set; }
    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return this.RedirectToPage("/Index");
        }

        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{userId}'.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await this.userManager.ConfirmEmailAsync(user, code);
        this.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        return this.Page();
    }
}
