using System.ComponentModel.DataAnnotations;
using IdSubjects;
using IdSubjects.ChineseName;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.UserManagement.Pages.Detail;

public class EditPersonNameModel(ApplicationUserManager<ApplicationUser> applicationUserManager) : PageModel
{
    public InputModel Input { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        ApplicationUser? person = await applicationUserManager.FindByIdAsync(anchor);
        if (person == null) return NotFound();
        Input = new InputModel
        {
            Surname = person.PersonName.Surname,
            GivenName = person.PersonName.GivenName ?? null!,
            PinyinSurname = person.PhoneticSurname,
            PinyinGivenName = person.PhoneticGivenName ?? null!
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        ApplicationUser? person = await applicationUserManager.FindByIdAsync(anchor);
        if (person == null) return NotFound();

        if (!ModelState.IsValid)
            return Page();

        var chinesePersonName =
            new ChinesePersonName(Input.Surname, Input.GivenName, Input.PinyinSurname, Input.PinyinGivenName);
        var personName = new PersonNameInfo(chinesePersonName.FullName, chinesePersonName.Surname,
            chinesePersonName.GivenName);
        await applicationUserManager.AdminChangePersonNameAsync(person, personName);
        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [Display(Name = "Surname")]
        [StringLength(10, ErrorMessage = "Validate_StringLength")]
        public string? Surname { get; set; }

        [Display(Name = "Given name")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(10, ErrorMessage = "Validate_StringLength")]
        public string GivenName { get; set; } = null!;

        [Display(Name = "Phonetic surname")]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        public string? PinyinSurname { get; set; }

        [Display(Name = "Phonetic given name")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(30, ErrorMessage = "Validate_StringLength")]
        public string PinyinGivenName { get; set; } = null!;
    }
}