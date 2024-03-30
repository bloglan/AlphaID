using AlphaIdPlatform.Platform;
using IdSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdminWebApp.Areas.People.Pages.Detail.Account;

public class ResetPasswordModel(NaturalPersonManager userManager, IShortMessageService shortMessageService, IOptions<IdentityOptions> options) : PageModel
{
    private readonly IdentityOptions _identityOptions = options.Value;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public NaturalPerson Person { get; set; } = default!;

    public string? OperationResult { get; set; }


    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var person = await userManager.FindByIdAsync(anchor);
        if (person == null)
        {
            return NotFound();
        }
        Person = person;
        Input = new();
        return Page();
    }

    public async Task<IActionResult> OnPostAutoReset(string anchor)
    {
        var person = await userManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();

        Person = person;

        if (string.IsNullOrEmpty(Person.PhoneNumber))
        {
            OperationResult = "无法通过短信重置密码。因为用户没有留下移动电话号码。";
            return Page();
        }

        var password = GeneratePassword();
        var result = await userManager.AdminResetPasswordAsync(Person, password, true, true);
        if (result.Succeeded)
        {
            await shortMessageService.SendAsync(Person.PhoneNumber, $"您的初始密码是[{password}]（不包括方括号）");
            OperationResult = "密码已重置并告知用户。";
            return Page();
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return Page();
        }
    }

    public async Task<IActionResult> OnPostManualReset(string anchor)
    {
        var person = await userManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();
        Person = person;

        var result = await userManager.AdminResetPasswordAsync(Person, Input.NewPassword, Input.UserMustChangePasswordOnNextLogin, Input.UnlockUser);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return Page();
        }


        OperationResult = "操作已成功。";
        return Page();
    }

    private string GeneratePassword()
    {
        var sb = new StringBuilder();
        var charset = new StringBuilder();
        if (_identityOptions.Password.RequireLowercase)
        {
            sb.Append(LowerCharset[Random.Shared.Next(LowerCharset.Length)]);
            charset.Append(LowerCharset);
        }
        if (_identityOptions.Password.RequireUppercase)
        {
            sb.Append(UpperCharset[Random.Shared.Next(UpperCharset.Length)]);
            charset.Append(UpperCharset);
        }
        if (_identityOptions.Password.RequireDigit)
        {
            sb.Append(NumCharset[Random.Shared.Next(NumCharset.Length)]);
            charset.Append(NumCharset);
        }
        if (_identityOptions.Password.RequireNonAlphanumeric)
        {
            sb.Append(SymbolCharset[Random.Shared.Next(SymbolCharset.Length)]);
            charset.Append(SymbolCharset);
        }
        //补全长度
        while (sb.Length < _identityOptions.Password.RequiredLength)
        {
            sb.Append(charset[Random.Shared.Next(charset.Length)]);
        }
        //洗牌
        Shuffle(sb);
        return sb.ToString();
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    /// <param name="sb"></param>
    /// <returns></returns>
    private static void Shuffle(StringBuilder sb)
    {
        for (int i = 0; i < sb.Length - 1; i++)
        {
            var p = i + 1 + Random.Shared.Next(sb.Length - i - 1);
            //swap
            (sb[p], sb[i]) = (sb[i], sb[p]);
        }
    }

    private const string UpperCharset = @"ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string LowerCharset = @"abcdefghijkmnopqrstuvwxyz";
    private const string NumCharset = @"123456789";
    private const string SymbolCharset = @"!@#$%^&*()_-+={}|[]\:;<>?,./";

    public class InputModel
    {
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        public string NewPassword { get; set; } = default!;

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        [Compare(nameof(NewPassword), ErrorMessage = "Validate_PasswordConfirm")]
        public string ConfirmPassword { get; set; } = default!;

        [Display(Name = "User must change password on next login")]
        public bool UserMustChangePasswordOnNextLogin { get; set; } = true;

        [Display(Name = "Unlock user (if locked)")]
        public bool UnlockUser { get; set; } = true;
    }
}
