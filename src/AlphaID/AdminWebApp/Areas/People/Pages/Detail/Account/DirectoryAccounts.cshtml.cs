using IdSubjects;
using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.People.Pages.Detail.Account;

public class DirectoryAccountsModel : PageModel
{
    private readonly NaturalPersonManager personManager;
    private readonly DirectoryAccountManager directoryAccountManager;

    public DirectoryAccountsModel(NaturalPersonManager personManager, DirectoryAccountManager directoryAccountManager)
    {
        this.personManager = personManager;
        this.directoryAccountManager = directoryAccountManager;
    }

    public NaturalPerson Person { get; set; } = default!;

    public IEnumerable<DirectoryAccount> LogonAccounts { get; set; } = default!;

    public async Task<IActionResult> OnGet(string anchor)
    {
        var person = await this.personManager.FindByIdAsync(anchor);
        if (person == null)
            return this.NotFound();

        this.Person = person;
        this.LogonAccounts = this.directoryAccountManager.GetLogonAccounts(this.Person);
        return this.Page();
    }
}
