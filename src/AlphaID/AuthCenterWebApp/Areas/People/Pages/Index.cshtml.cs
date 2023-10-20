using IDSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace AuthCenterWebApp.Areas.People.Pages
{
    public class IndexModel : PageModel
    {
        NaturalPersonManager personManager;

        public IndexModel(NaturalPersonManager personManager)
        {
            this.personManager = personManager;
        }

        public NaturalPerson Person { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string username)
        {
            var person = await this.personManager.FindByNameAsync(username);
            if (person == null)
                return this.NotFound();
            this.Person = person;
            return this.Page();
        }
    }
}
