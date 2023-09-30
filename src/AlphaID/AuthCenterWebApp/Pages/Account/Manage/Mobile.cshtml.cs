using AlphaIDPlatform.Platform;
using Duende.IdentityServer.Extensions;
using IDSubjects;
using IDSubjects.Subjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Pages.Account.Manage;

public class MobileModel : PageModel
{
    private readonly NaturalPersonManager userManager;
    private readonly IVerificationCodeService verificationCodeService;

    public MobileModel(NaturalPersonManager userManager, IVerificationCodeService verificationCodeService)
    {
        this.userManager = userManager;
        this.verificationCodeService = verificationCodeService;
    }

    [Display(Name = "移动电话号码")]
    public string Mobile { get; set; } = default!;

    public bool MobileValid { get; set; }

    [Display(Name = "新移动电话号码")]
    [BindProperty]
    [Required(ErrorMessage = "{0}是必需的")]
    public string NewMobile { get; set; } = default!;

    [Display(Name = "短信验证码")]
    [BindProperty]
    [Required(ErrorMessage = "{0}是必需的")]
    public string VerificationCode { get; set; } = default!;

    public bool VerificationCodeSent { get; set; }

    public string? OperationMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var person = await this.userManager.FindByIdAsync(this.User.GetSubjectId());
        if (person == null)
            return this.BadRequest("无法处理用户Id.");

        this.MobileValid = person.PhoneNumberConfirmed;
        this.Mobile = person.Mobile ?? "";
        this.NewMobile = person.Mobile ?? "";

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var person = await this.userManager.FindByIdAsync(this.User.GetSubjectId());
        if (person == null)
            return this.BadRequest("无法处理用户Id.");

        if (!MobilePhoneNumber.TryParse(this.NewMobile, out var phoneNumber))
        {
            this.ModelState.AddModelError(nameof(this.NewMobile), "移动电话号码无效。");
            return this.Page();
        }
        if (!await this.verificationCodeService.VerifyAsync(phoneNumber.ToString(), this.VerificationCode))
        {
            this.ModelState.AddModelError(nameof(this.VerificationCode), "验证码无效。");
            return this.Page();
        }

        var result = await this.userManager.SetPhoneNumberAsync(person, this.NewMobile);
        if (result.Succeeded)
        {
            person.PhoneNumberConfirmed = true;
            await this.userManager.UpdateAsync(person);
            this.OperationMessage = "移动电话号码已变更。";
            return this.Page();
        }

        this.OperationMessage = "无法变更移动电话号码。";
        return this.Page();
    }

    public async Task<IActionResult> OnPostSendVerificationCode()
    {
        if (!MobilePhoneNumber.TryParse(this.NewMobile, out var phoneNumber))
        {
            this.ModelState.AddModelError(nameof(this.NewMobile), "移动电话号码无效。");
            return this.Page();
        }

        await this.verificationCodeService.SendAsync(phoneNumber.ToString());

        this.VerificationCodeSent = true;
        return this.Page();
    }
}
