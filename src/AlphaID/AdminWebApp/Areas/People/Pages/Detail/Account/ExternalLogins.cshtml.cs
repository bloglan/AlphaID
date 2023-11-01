using IDSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.People.Pages.Detail.Account
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly NaturalPersonManager personManager;

        public ExternalLoginsModel(NaturalPersonManager personManager)
        {
            this.personManager = personManager;
        }

        public IEnumerable<UserLoginInfo> Logins { get; set; } = default!;

        public async Task<IActionResult> OnGet(string anchor)
        {
            var person = await this.personManager.FindByIdAsync(anchor);
            if (person == null) { return this.NotFound(); }

            this.Logins = await this.personManager.GetLoginsAsync(person);

            return this.Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(string anchor, string provider, string pkey)
        {
            var person = await this.personManager.FindByIdAsync(anchor);
            if (person == null) { return this.NotFound(); }

            var result = await this.personManager.RemoveLoginAsync(person, provider, pkey);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error.Description);
                }
            }
            return this.RedirectToPage();
        }
    }
}