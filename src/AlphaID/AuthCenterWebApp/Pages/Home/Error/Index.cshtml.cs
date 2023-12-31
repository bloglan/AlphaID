using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Pages.Home.Error;

[AllowAnonymous]
[SecurityHeaders]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService interaction;
    private readonly IWebHostEnvironment environment;

    public ViewModel View { get; set; } = default!;

    public Index(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
    {
        this.interaction = interaction;
        this.environment = environment;
    }

    public async Task OnGet(string errorId)
    {
        this.View = new ViewModel();

        // retrieve error details from identity server
        var message = await this.interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            this.View.Error = message;

            if (!this.environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }
    }
}