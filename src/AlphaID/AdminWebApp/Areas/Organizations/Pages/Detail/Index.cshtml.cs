using IdSubjects;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.Organizations.Pages.Detail;

public class IndexModel(OrganizationManager organizationManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Anchor { get; set; } = default!;

    public GenericOrganization Organization { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var org = await organizationManager.FindByIdAsync(this.Anchor);
        if (org == null)
            return this.NotFound();
        this.Organization = org;
        return this.Page();
    }

}
