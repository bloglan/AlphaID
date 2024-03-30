using AlphaIdPlatform.Platform;
using Duende.IdentityServer.Extensions;
using IdSubjects;
using IdSubjects.Subjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Areas.Settings.Pages.Account;

public class MobileModel(NaturalPersonManager userManager, IVerificationCodeService verificationCodeService) : PageModel
{
    [Display(Name = "PhoneNumber phone number")]
    public string Mobile { get; set; } = default!;

    public bool MobileValid { get; set; }

    [Display(Name = "New mobile phone number")]
    [BindProperty]
    [Required(ErrorMessage = "Validate_Required")]
    public string NewMobile { get; set; } = default!;

    [Display(Name = "Verification code")]
    [BindProperty]
    [Required(ErrorMessage = "Validate_Required")]
    public string VerificationCode { get; set; } = default!;

    public bool VerificationCodeSent { get; set; }

    public string? OperationMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var person = await userManager.FindByIdAsync(User.GetSubjectId());
        if (person == null)
            return BadRequest("无法处理用户Id.");

        MobileValid = person.PhoneNumberConfirmed;
        Mobile = person.PhoneNumber ?? "";
        NewMobile = person.PhoneNumber ?? "";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var person = await userManager.FindByIdAsync(User.GetSubjectId());
        if (person == null)
            return BadRequest("无法处理用户Id.");

        if (!MobilePhoneNumber.TryParse(NewMobile, out var phoneNumber))
        {
            ModelState.AddModelError(nameof(NewMobile), "移动电话号码无效。");
            return Page();
        }
        if (!await verificationCodeService.VerifyAsync(phoneNumber.ToString(), VerificationCode))
        {
            ModelState.AddModelError(nameof(VerificationCode), "验证码无效。");
            return Page();
        }

        var result = await userManager.SetPhoneNumberAsync(person, NewMobile);
        if (result.Succeeded)
        {
            OperationMessage = "移动电话号码已变更。";
            return Page();
        }

        OperationMessage = "无法变更移动电话号码。";
        return Page();
    }

    public async Task<IActionResult> OnPostSendVerificationCode()
    {
        if (!MobilePhoneNumber.TryParse(NewMobile, out var phoneNumber))
        {
            ModelState.AddModelError(nameof(NewMobile), "移动电话号码无效。");
            return Page();
        }

        await verificationCodeService.SendAsync(phoneNumber.ToString());

        VerificationCodeSent = true;
        return Page();
    }
}
