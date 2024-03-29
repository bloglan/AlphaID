using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Areas.Organization.Pages.Settings.Identifiers
{
    public class AddModel(OrganizationIdentifierManager identifierManager, OrganizationManager organizationManager) : PageModel
    {
        [BindProperty]
        [Display(Name = "Identifier Type")]
        public OrganizationIdentifierType Type { get; set; }

        [BindProperty]
        [Display(Name = "Identifier Value")]
        public string Value { get; set; } = default!;

        public IdOperationResult? Result { get; set; }

        public IActionResult OnGet(string anchor)
        {
            if (!organizationManager.TryGetSingleOrDefaultOrganization(anchor, out var organization))
                return this.RedirectToPage("/Who", new { anchor });
            if (organization == null)
                return this.NotFound();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string anchor)
        {
            if (!organizationManager.TryGetSingleOrDefaultOrganization(anchor, out var organization))
                return this.RedirectToPage("/Who", new { anchor });
            if (organization == null)
                return this.NotFound();

            if (!this.ModelState.IsValid)
                return this.Page();

            var identifier = new OrganizationIdentifier()
            {
                Organization = organization, OrganizationId = organization.Id, Type = this.Type, Value = this.Value,
            };

            this.Result = await identifierManager.AddIdentifierAsync(identifier);
            if (this.Result.Succeeded)
                return this.RedirectToPage("Index", new { anchor });

            return this.Page();
        }
    }
}
