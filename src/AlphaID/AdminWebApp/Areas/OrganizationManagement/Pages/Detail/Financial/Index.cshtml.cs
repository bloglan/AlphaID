using IdSubjects;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.OrganizationManagement.Pages.Detail.Financial;

public class IndexModel(OrganizationManager organizationManager, OrganizationBankAccountManager bankAccountManager)
    : PageModel
{
    public GenericOrganization Data { get; set; } = default!;

    public IEnumerable<OrganizationBankAccount> BankAccounts { get; set; } = [];

    public IdOperationResult? Result { get; set; }

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        GenericOrganization? data = await organizationManager.FindByIdAsync(anchor);
        if (data == null)
            return NotFound();
        Data = data;
        BankAccounts = bankAccountManager.GetBankAccounts(data);
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveAsync(string anchor, string accountNumber)
    {
        GenericOrganization? data = await organizationManager.FindByIdAsync(anchor);
        if (data == null)
            return NotFound();
        Data = data;
        BankAccounts = bankAccountManager.GetBankAccounts(data);

        OrganizationBankAccount? bankAccount = BankAccounts.FirstOrDefault(b => b.AccountNumber == accountNumber);
        if (bankAccount == null) return Page();

        Result = await bankAccountManager.RemoveAsync(bankAccount);
        if (Result.Succeeded)
            BankAccounts = bankAccountManager.GetBankAccounts(data);
        return Page();
    }

    public async Task<IActionResult> OnPostSetDefaultAsync(string anchor, string accountNumber)
    {
        GenericOrganization? data = await organizationManager.FindByIdAsync(anchor);
        if (data == null)
            return NotFound();
        Data = data;
        BankAccounts = bankAccountManager.GetBankAccounts(data);

        OrganizationBankAccount? bankAccount = BankAccounts.FirstOrDefault(b => b.AccountNumber == accountNumber);
        if (bankAccount == null) return Page();

        Result = await bankAccountManager.SetDefault(bankAccount);
        if (Result.Succeeded)
            BankAccounts = bankAccountManager.GetBankAccounts(data);
        return Page();
    }
}