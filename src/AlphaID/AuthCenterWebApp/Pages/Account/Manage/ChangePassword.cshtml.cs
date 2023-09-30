﻿#nullable disable

using IDSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Pages.Account.Manage;

public class ChangePasswordModel : PageModel
{
    private readonly NaturalPersonManager _userManager;
    private readonly SignInManager<NaturalPerson> _signInManager;
    private readonly ILogger<ChangePasswordModel> _logger;

    public ChangePasswordModel(
        NaturalPersonManager userManager,
        SignInManager<NaturalPerson> signInManager,
        ILogger<ChangePasswordModel> logger)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public NaturalPerson Person { get; set; } = default!;

    public IList<UserLoginInfo> ExternalLogins { get; set; } = default!;

    public class InputModel
    {
        [Required(ErrorMessage = "{0}是必需的")]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0}是必需的")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }
        this.Person = user;
        this.ExternalLogins = await this._userManager.GetLoginsAsync(user);
        var hasPassword = await this._userManager.HasPasswordAsync(user);
        return !hasPassword ? this.RedirectToPage("./SetPassword") : this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }
        this.Person = user;

        var changePasswordResult = await this._userManager.ChangePasswordAsync(user, this.Input.OldPassword, this.Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
            return this.Page();
        }

        await this._signInManager.RefreshSignInAsync(user);
        this._logger.LogInformation("用户已成功更改其密码。");
        this.StatusMessage = "您的密码已更改。";

        return this.RedirectToPage();
    }
}
