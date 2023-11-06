using IDSubjects;
using IDSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail.Account;

public class CreateDirectoryAccountModel : PageModel
{
    private readonly DirectoryServiceManager directoryServiceManager;
    private readonly LogonAccountManager logonAccountManager;
    private readonly NaturalPersonManager naturalPersonManager;

    public CreateDirectoryAccountModel(DirectoryServiceManager directoryServiceManager, LogonAccountManager logonAccountManager, NaturalPersonManager naturalPersonManager)
    {
        this.directoryServiceManager = directoryServiceManager;
        this.logonAccountManager = logonAccountManager;
        this.naturalPersonManager = naturalPersonManager;
    }

    public IEnumerable<DirectoryService> DirectoryServices => this.directoryServiceManager.Services;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var person = await this.naturalPersonManager.FindByIdAsync(anchor);
        if (person == null)
            return this.NotFound();

        //准备姓名全拼+身份证后4位
        var accountName = $"{person.PhoneticSurname}{person.PhoneticGivenName}".ToLower();

        //准备有关资料
        this.Input = new()
        {
            SamAccountName = accountName,
            UpnPart = accountName,
            EntryName = accountName,
            Surname = person.LastName!,
            GivenName = person.FirstName!,
            DisplayName = person.Name,
            PinyinSurname = person.PhoneticSurname,
            PinyinGivenName = person.PhoneticGivenName,
            PinyinDisplayName = person.PhoneticSurname + person.PhoneticGivenName,
            Mobile = person.PhoneNumber!,
            Email = person.Email,
        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string anchor)
    {
        var person = await this.naturalPersonManager.FindByIdAsync(anchor);
        if (person == null)
            return this.NotFound();

        if (!this.ModelState.IsValid)
            return this.Page();

        var directoryService = this.directoryServiceManager.FindByIdAsync(this.Input.ServiceId);
        if (directoryService == null)
            this.ModelState.AddModelError("", "请选择一个目录服务");

        CreateAccountRequest request = new()
        {
            AccountName = this.Input.EntryName,
            SamAccountName = this.Input.SamAccountName,
            UpnLeftPart = this.Input.UpnPart,
            DisplayName = this.Input.DisplayName,
            Surname = this.Input.Surname,
            GivenName = this.Input.GivenName,
            Email = this.Input.Email,
            E164Mobile = this.Input.Mobile,
            ServiceId = this.Input.ServiceId,
            InitPassword = this.Input.NewPassword,
        };
        try
        {
            await this.logonAccountManager.CreateAsync(person, request);
            return this.RedirectToPage("DirectoryAccounts", new { anchor });
        }
        catch (Exception ex)
        {
            this.ModelState.AddModelError("", ex.Message);
            return this.Page();
        }
    }

    public class InputModel
    {
        [Display(Name = "Directory Service")]
        public int ServiceId { get; init; }

        [Display(Name = "SAM Account Name")]
        public string SamAccountName { get; init; } = default!;

        [Display(Name = "UPN Prefix Part")]
        public string UpnPart { get; init; } = default!;

        [Display(Name = "Directory Entry Name")]
        public string EntryName { get; init; } = default!;

        public string Surname { get; init; } = default!;

        public string GivenName { get; init; } = default!;

        public string DisplayName { get; init; } = default!;

        public string? PinyinSurname { get; init; }

        public string? PinyinGivenName { get; init; }

        public string? PinyinDisplayName { get; init; }

        [Display(Name = "PhoneNumber Phone Number")]
        public string Mobile { get; init; } = default!;

        [Display(Name = "Email Address")]
        public string? Email { get; init; }

        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        public string NewPassword { get; init; } = default!;

        [Display(Name = "Confirm Password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Validate_StringLength")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Validate_PasswordConfirm")]
        public string ConfirmPassword { get; init; } = default!;
    }
}
