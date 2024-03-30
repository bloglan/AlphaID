using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail.Account;

public class SetPasswordModel(NaturalPersonManager userManager) : PageModel
{
    [StringLength(30, ErrorMessage = "Validate_StringLength")]
    [DataType(DataType.Password)]
    [BindProperty]
    public string NewPassword { get; set; } = default!;

    [StringLength(30, ErrorMessage = "Validate_StringLength")]
    [Compare(nameof(NewPassword), ErrorMessage = "Validate_PasswordConfirm")]
    [DataType(DataType.Password)]
    [BindProperty]
    public string ConfirmPassword { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var person = await userManager.FindByIdAsync(anchor);
        return person == null
            ? NotFound()
            : await userManager.HasPasswordAsync(person) ? throw new InvalidOperationException("用户已具有密码，无法手动添加密码") : (IActionResult)Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        var person = await userManager.FindByIdAsync(anchor);
        if (person == null)
        {
            return NotFound();
        }

        if (await userManager.HasPasswordAsync(person))
        {
            throw new InvalidOperationException("用户已具有密码，无法手动添加密码");
        }

        var result = await userManager.AddPasswordAsync(person, NewPassword);
        if (result.Succeeded)
        {
            return RedirectToPage("SetPasswordSuccess", new { anchor });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return Page();
    }
}
