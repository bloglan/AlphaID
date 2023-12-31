using IdSubjects;
using IdSubjects.RealName;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthCenterWebApp.Areas.Settings.Pages.Profile
{
    public class PersonNameModel : PageModel
    {
        private readonly NaturalPersonManager personManager;
        RealNameManager realNameManager;
        public PersonNameModel(NaturalPersonManager personManager, RealNameManager realNameManager)
        {
            this.personManager = personManager;
            this.realNameManager = realNameManager;
        }

        [BindProperty]
        public InputMode Input { get; set; } = default!;

        public IdentityResult? Result { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var person = await this.personManager.GetUserAsync(this.User);
            if (person == null)
            {
                return this.NotFound();
            }

            var hasRealName = this.realNameManager.GetAuthentications(person).Any();
            if (hasRealName)
            {
                this.Result = IdentityResult.Failed(new IdentityError() { Code = "Cannot change name after real-name authentication", Description = "You cannot change name because your has been passed real-name authentication." });
            }

            this.Input = new InputMode()
            {
                Surname = person.PersonName.Surname,
                MiddleName = person.PersonName.MiddleName,
                GivenName = person.PersonName.GivenName,
                PhoneticSurname = person.PhoneticSurname,
                PhoneticGivenName = person.PhoneticGivenName,
                NickName = person.NickName,
            };
            return this.Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var person = await this.personManager.GetUserAsync(this.User);
            if (person == null)
            {
                return this.NotFound();
            }

            person.PersonName = new PersonNameInfo($"{this.Input.Surname}{this.Input.GivenName}", this.Input.Surname, this.Input.GivenName, this.Input.MiddleName);
            person.PhoneticSurname = this.Input.PhoneticSurname;
            person.PhoneticGivenName = this.Input.PhoneticGivenName;
            person.NickName = this.Input.NickName;

            this.Result = await this.personManager.UpdateAsync(person);
            return this.Page();
        }

        public class InputMode
        {
            [Display(Name = "Surname")]
            [StringLength(10)]
            public string? Surname { get; set; }

            [Display(Name = "MiddleName")]
            [StringLength(10)]
            public string? MiddleName { get; set; }

            [Display(Name = "GivenName")]
            [StringLength(10)]
            public string? GivenName { get; set; }

            [Display(Name = "PhoneticSurname")]
            [StringLength(10)]
            public string? PhoneticSurname { get; set; }

            [Display(Name = "PhoneticGivenName")]
            [StringLength(10)]
            public string? PhoneticGivenName { get; set; }

            [Display(Name = "NickName")]
            [StringLength(10)]
            public string? NickName { get; set; }
        }
    }
}
