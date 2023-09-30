using IDSubjects;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail;

public class DeleteModel : PageModel
{
    private readonly NaturalPersonManager userManager;

    public DeleteModel(NaturalPersonManager userManager)
    {
        this.userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; } = default!;

    public NaturalPerson Person { get; set; } = default!;

    [BindProperty]
    public DeletePersonForm Input { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var person = await this.userManager.FindByIdAsync(this.Id.ToString());
        if (person == null)
            return this.NotFound();
        this.Person = person;
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var person = await this.userManager.FindByIdAsync(this.Id);
        if (person == null)
            return this.NotFound();
        this.Person = person;


        if (this.Input.DisplayName != this.Person.Name)
        {
            this.ModelState.AddModelError(nameof(this.Input.DisplayName), "名称不一致");
        }

        if (!this.ModelState.IsValid)
            return this.Page();

        try
        {
            var result = await this.userManager.DeleteAsync(this.Person);
            if (result.Succeeded)
                return this.RedirectToPage("DeleteSuccess");
            else
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error.Description);
                }
                return this.Page();
            }
        }
        catch (Exception ex)
        {
            this.ModelState.TryAddModelException("", ex);
            return this.Page();
        }
    }

    public class DeletePersonForm
    {

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}是必需的")]
        public string DisplayName { get; set; } = default!;
    }
}
