using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.OpenIDConnect.Pages.Clients.Detail;

public class IndexModel(ConfigurationDbContext dbContext) : PageModel
{
    public Client Client { get; set; } = default!;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public string? OperationMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int anchor)
    {
        var data = await this.GetClient(anchor);
        if (data == null)
            return this.NotFound();
        this.Client = data;
        this.Input = new InputModel
        {
            ClientId = this.Client.ClientId,
            ClientName = this.Client.ClientName,
            Description = this.Client.Description,
            Enabled = this.Client.Enabled,
            ClientUri = this.Client.ClientUri,
            LogoUri = this.Client.LogoUri,
            RequireClientSecret = this.Client.RequireClientSecret,
        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int anchor)
    {
        var data = await this.GetClient(anchor);
        if (data == null)
            return this.NotFound();
        this.Client = data;

        if (this.Input.ClientId != this.Client.ClientId && dbContext.Clients.Any(p => p.ClientId == this.Input.ClientId))
            this.ModelState.AddModelError("", "Client Anchor �Ѵ��ڡ�");

        if (!this.ModelState.IsValid)
            return this.Page();

        this.Client.ClientName = this.Input.ClientName;
        this.Client.ClientId = this.Input.ClientId;
        this.Client.Description = this.Input.Description;
        this.Client.Enabled = this.Input.Enabled;
        this.Client.LogoUri = this.Input.LogoUri;
        this.Client.ClientUri = this.Input.ClientUri;
        this.Client.RequireClientSecret = this.Input.RequireClientSecret;

        dbContext.Clients.Update(this.Client);
        await dbContext.SaveChangesAsync();
        this.OperationMessage = "Applied";
        return this.Page();
    }

    private Task<Client?> GetClient(int anchor)
    {
        return dbContext.Clients
            .Include(p => p.AllowedScopes)
            .Include(p => p.AllowedGrantTypes)
            .Include(p => p.RedirectUris)
            .Include(p => p.PostLogoutRedirectUris)
            .Include(p => p.AllowedCorsOrigins)
            .Include(p => p.IdentityProviderRestrictions)
            .Include(p => p.Properties)
            .AsSingleQuery()
            .SingleOrDefaultAsync(p => p.Id == anchor);
    }

    public class InputModel
    {
        [Display(Name = "Client name")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(200, ErrorMessage = "Validate_StringLength")]
        public string ClientName { get; set; } = default!;

        [Display(Name = "Client ID")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(200, ErrorMessage = "Validate_StringLength")]
        public string ClientId { get; set; } = default!;

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Validate_StringLength")]
        public string? Description { get; set; }

        [Display(Name = "Enabled")]
        public bool Enabled { get; set; }

        [Display(Name = "Logo URI")]
        [DataType(DataType.Url)]
        public string? LogoUri { get; set; }

        [Display(Name = "Client URI")]
        [DataType(DataType.Url)]
        public string? ClientUri { get; set; }

        [Display(Name = "Require client secret")]
        public bool RequireClientSecret { get; set; }
    }
}
