using System.ComponentModel.DataAnnotations;
using AlphaIdPlatform.Identity;
using IdSubjects;
using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.UserManagement.Pages.Detail.Account;

public class CreateDirectoryAccountModel(
    DirectoryServiceManager directoryServiceManager,
    DirectoryAccountManager<NaturalPerson> directoryAccountManager,
    UserManager<NaturalPerson> applicationUserManager) : PageModel
{
    public IEnumerable<DirectoryServiceDescriptor> DirectoryServices => directoryServiceManager.Services;

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        ApplicationUser? person = await applicationUserManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();

        //准备姓名全拼+身份证后4位
        string accountName = $"{person.PhoneticSurname}{person.PhoneticGivenName}".ToLower();

        //准备有关资料
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
        ApplicationUser? person = await applicationUserManager.FindByIdAsync(anchor);
        if (person == null)
            return NotFound();

        DirectoryServiceDescriptor? directoryService = await directoryServiceManager.FindByIdAsync(Input.ServiceId);
        if (directoryService == null)
            ModelState.AddModelError("", "请选择一个目录服务");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var logonAccount = new DirectoryAccount(directoryService!, person.Id);
            await directoryAccountManager.CreateAsync(logonAccount);
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
        public string SamAccountName { get; set; } = null!;

        [Display(Name = "UPN Prefix Part")]
        [Required(ErrorMessage = "Validate_Required")]
        public string UpnPart { get; set; } = null!;

        [Display(Name = "Directory Entry Name")]
        [Required(ErrorMessage = "Validate_Required")]
        public string EntryName { get; set; } = null!;

        [Required(ErrorMessage = "Validate_Required")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = "Validate_Required")]
        public string GivenName { get; set; } = null!;

        [Required(ErrorMessage = "Validate_Required")]
        public string DisplayName { get; set; } = null!;

        public string? PinyinSurname { get; set; }

        public string? PinyinGivenName { get; set; }

        public string? PinyinDisplayName { get; set; }

        [Display(Name = "PhoneNumber Phone Number")]
        [Required(ErrorMessage = "Validate_Required")]
        public string Mobile { get; set; } = null!;

        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Validate_Required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Validate_PasswordConfirm")]
        public string ConfirmPassword { get; set; } = null!;
    }
}