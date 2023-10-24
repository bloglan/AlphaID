﻿#nullable disable

using AlphaIDPlatform;
using AlphaIDPlatform.Platform;
using IDSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthCenterWebApp.Areas.MyAccount.Pages;

public class EmailModel : PageModel
{
    private readonly NaturalPersonManager _userManager;
    private readonly IEmailSender _emailSender;
    private readonly ProductInfo production;

    public EmailModel(
        NaturalPersonManager userManager,
        IEmailSender emailSender,
        IOptions<ProductInfo> production)
    {
        this._userManager = userManager;
        this._emailSender = emailSender;
        this.production = production.Value;
    }

    [Display(Name = "Email")]
    public string Email { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    /// </summary>
    public class InputModel
    {
        [Required(ErrorMessage = "Validate_Required")]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }

    private async Task LoadAsync(NaturalPerson user)
    {
        var email = await this._userManager.GetEmailAsync(user);
        this.Email = email;

        this.Input = new InputModel
        {
            NewEmail = email,
        };

        this.IsEmailConfirmed = await this._userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        await this.LoadAsync(user);
        return this.Page();
    }

    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        if (!this.ModelState.IsValid)
        {
            await this.LoadAsync(user);
            return this.Page();
        }

        var email = await this._userManager.GetEmailAsync(user);
        if (this.Input.NewEmail != email)
        {
            var userId = await this._userManager.GetUserIdAsync(user);
            var code = await this._userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new { area = "", userId, email = this.Input.NewEmail, code },
                protocol: this.Request.Scheme);
            await this._emailSender.SendEmailAsync(
                this.Input.NewEmail,
                "确认您的邮件地址",
                $"<p>您已请求更改电子邮件地址，请单击<a href='{callbackUrl}'>这里</a>以确认您的邮件地址。</p>" +
                $"<p>{this.production.Name}团队</p>");

            this.StatusMessage = "变更电子邮件的确认链接已发至您的新邮箱，请到您的邮箱查收邮件并完成变更确认。";
            return this.RedirectToPage();
        }

        this.StatusMessage = "您的邮件地址未更改。";
        return this.RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        if (!this.ModelState.IsValid)
        {
            await this.LoadAsync(user);
            return this.Page();
        }

        var userId = await this._userManager.GetUserIdAsync(user);
        var email = await this._userManager.GetEmailAsync(user);
        var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = this.Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId, code },
            protocol: this.Request.Scheme);
        await this._emailSender.SendEmailAsync(
            email,
            "确认您的邮件地址",
            $"<p>您已请求更改电子邮件地址，请单击<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>这里</a>以确认您的邮件地址。</p>" +
            $"<p>{this.production.Name}团队</p>");

        this.StatusMessage = "验证邮件已发送，请到您的邮箱检查邮件。";
        return this.RedirectToPage();
    }
}
