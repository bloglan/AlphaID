using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Pages.Redirect;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public string RedirectUri { get; set; } = default!;

    public IActionResult OnGet(string redirectUri)
    {
        if (!Url.IsLocalUrl(redirectUri))
        {
            return RedirectToPage("/Home/Error/LoginModel");
        }

        RedirectUri = redirectUri;
        return Page();
    }
}