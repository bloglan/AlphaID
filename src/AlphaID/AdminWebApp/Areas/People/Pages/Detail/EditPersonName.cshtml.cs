using IdSubjects;
using IdSubjects.ChineseName;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail;

public class EditPersonNameModel(NaturalPersonManager naturalPersonManager) : PageModel
{
    public InputModel Input { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var person = await naturalPersonManager.FindByIdAsync(anchor);
        if (person == null)
        {
            return this.NotFound();
        }
        this.Input = new()
        {
            Surname = person.PersonName.Surname,
            GivenName = person.PersonName.GivenName ?? default!,
            PinyinSurname = person.PhoneticSurname,
            PinyinGivenName = person.PhoneticGivenName ?? default!,
        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        var person = await naturalPersonManager.FindByIdAsync(anchor);
        if (person == null) { return this.NotFound(); }

        if (!this.ModelState.IsValid)
            return this.Page();

        var chinesePersonName = new ChinesePersonName(this.Input.Surname, this.Input.GivenName, this.Input.PinyinSurname, this.Input.PinyinGivenName);
        var personName = new PersonNameInfo(chinesePersonName.FullName, chinesePersonName.Surname, chinesePersonName.GivenName);
        await naturalPersonManager.AdminChangePersonNameAsync(person, personName);
        return this.RedirectToPage("Index");
    }

    public class InputModel
    {
        [Display(Name = "Surname")]
        [StringLength(10, ErrorMessage = "Validate_StringLength")]
        public string? Surname { get; set; }

        [Display(Name = "Given name")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(10, ErrorMessage = "Validate_StringLength")]
        public string GivenName { get; set; } = default!;

        [Display(Name = "Phonetic surname")]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        public string? PinyinSurname { get; set; }

        [Display(Name = "Phonetic given name")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        public string PinyinGivenName { get; set; } = default!;
    }
}
