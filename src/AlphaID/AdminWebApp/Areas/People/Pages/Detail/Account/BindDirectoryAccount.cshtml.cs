using IdSubjects;
using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.People.Pages.Detail.Account;

public class BindDirectoryAccountModel(NaturalPersonManager personManager, DirectoryAccountManager directoryAccountManager, DirectoryServiceManager directoryServiceManager) : PageModel
{
    public IEnumerable<DirectoryServiceDescriptor> DirectoryServices => directoryServiceManager.Services;

    public NaturalPerson Person { get; set; } = default!;

    public IEnumerable<DirectorySearchItem> SearchItems { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var person = await personManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();
        Person = person;
        return Page();
    }

    public async Task<IActionResult> OnPostSearchAsync(string anchor, int serviceId, string keywords)
    {
        var person = await personManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();
        Person = person;

        var directoryService = await directoryServiceManager.FindByIdAsync(serviceId);
        if (directoryService == null)
            return Page();

        SearchItems = directoryAccountManager.Search(directoryService, $"(anr={keywords})");
        return Page();
    }

    public async Task<IActionResult> OnPostBindAsync(string anchor, int serviceId, Guid entryGuid)
    {
        var person = await personManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();
        Person = person;

        var directoryService = await directoryServiceManager.FindByIdAsync(serviceId);
        if (directoryService == null)
            return Page();
        var logonAccount = new DirectoryAccount(directoryService, person.Id);
        await directoryAccountManager.BindExistsAccount(personManager, logonAccount, entryGuid.ToString());
        return RedirectToPage("DirectoryAccounts", new { anchor });
    }
}
