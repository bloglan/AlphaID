using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut(IIdentityServerInteractionService interactionService) : PageModel
{
    public LoggedOutViewModel View { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public async Task OnGet(string? logoutId, string? returnUrl)
    {
        this.ReturnUrl = returnUrl ?? "/";
        // get context information (client name, post logout redirect URI and iframe for federated sign out)
        var logout = await interactionService.GetLogoutContextAsync(logoutId);

        this.View = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout.ClientName) ? logout.ClientId : logout.ClientName,
            SignOutIframeUrl = logout.SignOutIFrameUrl
        };
    }
}