using System.ComponentModel.DataAnnotations;
using IdSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.UserManagement.Pages.Detail;

public class DeleteModel(NaturalPersonManager userManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Anchor { get; set; } = default!;

    public NaturalPerson Person { get; set; } = default!;

    [BindProperty]
    public DeletePersonForm Input { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        NaturalPerson? person = await userManager.FindByIdAsync(Anchor);
        if (person == null)
            return NotFound();
        Person = person;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        NaturalPerson? person = await userManager.FindByIdAsync(Anchor);
        if (person == null)
            return NotFound();
        Person = person;


        if (Input.DisplayName != Person.PersonName.FullName)
            ModelState.AddModelError(nameof(Input.DisplayName), "���Ʋ�һ��");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            IdentityResult result = await userManager.DeleteAsync(Person);
            if (result.Succeeded)
            {
                return RedirectToPage("DeleteSuccess");
            }

            foreach (IdentityError error in result.Errors) ModelState.AddModelError("", error.Description);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.TryAddModelException("", ex);
            return Page();
        }
    }

    public class DeletePersonForm
    {
        [Display(Name = "Display name", Description = "A friendly name that appears on the user interface.")]
        [Required(ErrorMessage = "Validate_Required")]
        public string DisplayName { get; set; } = default!;
    }
}