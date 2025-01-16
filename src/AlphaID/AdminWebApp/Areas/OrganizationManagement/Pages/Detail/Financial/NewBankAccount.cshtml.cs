using System.ComponentModel.DataAnnotations;
using IdSubjects;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.OrganizationManagement.Pages.Detail.Financial;

public class NewBankAccountModel(
    OrganizationManager organizationManager,
    OrganizationBankAccountManager bankAccountManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public IdOperationResult? Result { get; set; }

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        Organization? org = await organizationManager.FindByIdAsync(anchor);
        if (org == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        Organization? org = await organizationManager.FindByIdAsync(anchor);
        if (org == null)
            return NotFound();

        if (!ModelState.IsValid)
            return Page();

        Result = await bankAccountManager.AddAsync(org, Input.AccountNumber, Input.AccountName,
            Input.BankName, Input.Usage, Input.SetDefault);

        if (!Result.Succeeded)
            return Page();

        return RedirectToPage("Index", new { anchor });
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Validate_Required")]
        public string AccountNumber { get; set; } = null!;

        [Required(ErrorMessage = "Validate_Required")]
        public string AccountName { get; set; } = null!;

        [Required(ErrorMessage = "Validate_Required")]
        public string BankName { get; set; } = null!;

        public string? Usage { get; set; }

        public bool SetDefault { get; set; } = false;
    }
}