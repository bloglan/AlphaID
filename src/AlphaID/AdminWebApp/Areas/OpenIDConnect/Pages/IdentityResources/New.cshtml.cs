using Castle.Components.DictionaryAdapter;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.OpenIDConnect.Pages.IdentityResources
{
    public class NewModel(ConfigurationDbContext dbContext) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public List<SelectListItem> AvaileableClaims { get; set; } = new EditableList<SelectListItem>()
        {
            new("sub", "sub"),
            new("name", "name"),
            new("given_name", "given_name"),
            new("family_name", "family_name"),
            new("middle_name", "middle_name"),
            new("nickname", "nickname"),
            new("preferred_username", "preferred_username"),
            new("profile", "profile"),
            new("picture", "picture"),
            new("website", "website"),
            new("email", "email"),
            new("email_verified", "email_verified"),
            new("gender", "gender"),
            new("birthdate", "birthdate"),
            new("zoneinfo", "zoneinfo"),
            new("locale", "locale"),
            new("phone_number", "phone_number"),
            new("phone_number_verified", "phone_number_verified"),
            new("address", "address"),
            new("updated_at", "updated_at"),
        };

        public void OnGet()
        {
            this.Input = new InputModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (dbContext.IdentityResources.Any(p => p.Name == this.Input.Name))
                this.ModelState.AddModelError("Name", "Name is exists.");

            if (!this.ModelState.IsValid)
                return this.Page();

            var identityResource = new IdentityResource
            {
                Enabled = true,
                Name = this.Input.Name,
                DisplayName = this.Input.DisplayName,
                Description = this.Input.Description,
                Required = this.Input.Required,
                Emphasize = this.Input.Emphasize,
                ShowInDiscoveryDocument = this.Input.ShowInDiscoveryDocument,
                UserClaims = null,
                Properties = null,
                Created = DateTime.Now,
                Updated = null,
                NonEditable = false,

            };
            dbContext.IdentityResources.Add(identityResource);
            await dbContext.SaveChangesAsync();
            return this.RedirectToPage("Index");
        }

        public record InputModel
        {
            [Display(Name = "Name")]
            [Required(ErrorMessage = "Validate_Required")]
            [StringLength(200, ErrorMessage = "Validate_StringLength")]
            public string Name { get; set; } = default!;

            [Display(Name = "Display name", Description = "A friendly name that appears on the user interface.")]
            [StringLength(200, ErrorMessage = "Validate_StringLength")]
            public string? DisplayName { get; set; }

            [Display(Name = "Description")]
            [StringLength(1000, ErrorMessage = "Validate_StringLength")]
            public string? Description { get; set; }

            [Display(Name = "Required")]
            public bool Required { get; set; } = false;

            [Display(Name = "Emphasize")]
            public bool Emphasize { get; set; } = false;

            [Display(Name = "Show in discovery document")]
            public bool ShowInDiscoveryDocument { get; set; } = true;

            public string[] SelectedClaims { get; set; } = [];
        }
    }
}
