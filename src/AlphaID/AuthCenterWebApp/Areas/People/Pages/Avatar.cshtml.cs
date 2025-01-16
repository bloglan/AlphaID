using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.People.Pages;

public class AvatarModel(ApplicationUserManager personManager) : PageModel
{
    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        ApplicationUser? person =
            await personManager.FindByNameAsync(anchor) ?? await personManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();
        if (person.ProfilePicture != null)
            return File(person.ProfilePicture.Data, person.ProfilePicture.MimeType);
        return File("~/img/no-picture-avatar.png", "image/png");
    }
}