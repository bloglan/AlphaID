using IdSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail
{
    public class AdvancedModel(NaturalPersonManager naturalPersonManager) : PageModel
    {
        public NaturalPerson Data { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IdentityResult? Result { get; set; }

        public async Task<IActionResult> OnGetAsync(string anchor)
        {
            var person = await naturalPersonManager.FindByIdAsync(anchor);
            if (person == null)
                return NotFound();
            Data = person;

            Input = new InputModel
            {
                Enabled = Data.Enabled,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string anchor)
        {
            var person = await naturalPersonManager.FindByIdAsync(anchor);
            if (person == null)
                return NotFound();
            Data = person;

            if (!ModelState.IsValid)
                return Page();

            Data.Enabled = Input.Enabled;

            Result = await naturalPersonManager.UpdateAsync(person);
            return Page();
        }

        public class InputModel
        {
            [Display(Name = "Enabled")]
            public bool Enabled { get; set; }
        }
    }
}
