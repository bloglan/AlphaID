using System.ComponentModel.DataAnnotations;
using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.Organization.Pages.Settings;

public class RenameModel(OrganizationManager manager) : PageModel
{
    [BindProperty]
    [Display(Name = "Name")]
    [StringLength(50, ErrorMessage = "Validate_StringLength")]
    public string Name { get; set; } = default!;

    public IdOperationResult? Result { get; set; }

    public IActionResult OnGet(string anchor)
    {
        if (!manager.TryGetSingleOrDefaultOrganization(anchor, out GenericOrganization? organization))
            return RedirectToPage("/Who", new { anchor });
        if (organization == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        if (!manager.TryGetSingleOrDefaultOrganization(anchor, out GenericOrganization? organization))
            return RedirectToPage("/Who", new { anchor });
        if (organization == null)
            return NotFound();

        if (manager.Organizations.Any(o => o.Name == Name))
            ModelState.AddModelError(nameof(Name), "The name is in use.");

        if (!ModelState.IsValid)
            return Page();

        Result = await manager.ChangeNameAsync(organization, Name, DateOnly.FromDateTime(DateTime.UtcNow), true);
        if (!Result.Succeeded)
            return Page();

        //Redirect with new name
        return RedirectToPage("/Index", new { anchor = Name });
    }
}