using System.ComponentModel.DataAnnotations;
using IdSubjects;
using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.UserManagement.Pages.Detail.Account;

public class CreateDirectoryAccountModel(
    DirectoryServiceManager directoryServiceManager,
    DirectoryAccountManager directoryAccountManager,
    NaturalPersonManager naturalPersonManager) : PageModel
{
    public IEnumerable<DirectoryServiceDescriptor> DirectoryServices => directoryServiceManager.Services;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        NaturalPerson? person = await naturalPersonManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();

        //׼������ȫƴ+����֤��4λ
        string accountName = $"{person.PhoneticSurname}{person.PhoneticGivenName}".ToLower();

        //׼���й�����
        Input = new InputModel
        {
            SamAccountName = accountName,
            UpnPart = accountName,
            EntryName = accountName,
            Surname = person.PersonName.Surname!,
            GivenName = person.PersonName.GivenName!,
            DisplayName = person.PersonName.FullName,
            PinyinSurname = person.PhoneticSurname,
            PinyinGivenName = person.PhoneticGivenName,
            PinyinDisplayName = person.PhoneticSurname + person.PhoneticGivenName,
            Mobile = person.PhoneNumber!,
            Email = person.Email
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        NaturalPerson? person = await naturalPersonManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();

        DirectoryServiceDescriptor? directoryService = await directoryServiceManager.FindByIdAsync(Input.ServiceId);
        if (directoryService == null)
            ModelState.AddModelError("", "��ѡ��һ��Ŀ¼����");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var logonAccount = new DirectoryAccount(directoryService!, person.Id);
            await directoryAccountManager.CreateAsync(naturalPersonManager, logonAccount);
            return RedirectToPage("DirectoryAccounts", new { anchor });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return Page();
        }
    }

    public class InputModel
    {
        [Display(Name = "Directory Service")]
        public int ServiceId { get; set; }

        [Display(Name = "SAM Account Name")]
        [Required(ErrorMessage = "Validate_Required")]
        public string SamAccountName { get; set; } = default!;

        [Display(Name = "UPN Prefix Part")]
        [Required(ErrorMessage = "Validate_Required")]
        public string UpnPart { get; set; } = default!;

        [Display(Name = "Directory Entry Name")]
        [Required(ErrorMessage = "Validate_Required")]
        public string EntryName { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        public string Surname { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        public string GivenName { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        public string DisplayName { get; set; } = default!;

        public string? PinyinSurname { get; set; }

        public string? PinyinGivenName { get; set; }

        public string? PinyinDisplayName { get; set; }

        [Display(Name = "PhoneNumber Phone Number")]
        [Required(ErrorMessage = "Validate_Required")]
        public string Mobile { get; set; } = default!;

        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Validate_PasswordConfirm")]
        public string ConfirmPassword { get; set; } = default!;
    }
}