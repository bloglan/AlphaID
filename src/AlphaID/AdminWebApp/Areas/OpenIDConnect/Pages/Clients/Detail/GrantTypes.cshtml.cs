using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWebApp.Areas.OpenIDConnect.Pages.Clients.Detail;

public class GrantTypesModel : PageModel
{
    private readonly ConfigurationDbContext dbContext;

    public GrantTypesModel(ConfigurationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Client Data { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        var data = this.dbContext.Clients.Include(p => p.AllowedGrantTypes).FirstOrDefault(p => p.Id == id);
        if (data == null)
            return this.NotFound();
        this.Data = data;
        return this.Page();
    }
}
