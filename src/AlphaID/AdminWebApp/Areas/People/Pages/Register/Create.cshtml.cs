using IdSubjects;
using IdSubjects.ChineseName;
using IdSubjects.Subjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Register;

public class CreateModel(ChinesePersonNamePinyinConverter pinyinConverter, NaturalPersonManager manager) : PageModel
{
    [BindProperty]
    [Display(Name = "User name")]
    [PageRemote(PageHandler = "CheckUserName", HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Validate_StringLength")]
    public string UserName { get; set; } = default!;

    [BindProperty]
    [Display(Name = "Phone number")]
    [PageRemote(PageHandler = "CheckMobile", HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "Validate_StringLength")]
    public string? Mobile { get; set; } = default!;

    [BindProperty]
    [Display(Name = "Email")]
    [PageRemote(PageHandler = "CheckEmail", HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "Validate_StringLength")]
    public string? Email { get; set; } = default!;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public IdentityResult? Result { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var builder = new PersonBuilder();
        if (this.Mobile != null)
        {
            if (MobilePhoneNumber.TryParse(this.Mobile, out var phoneNumber))
                builder.SetMobile(phoneNumber);
            else
                this.ModelState.AddModelError("", "移动电话号码无效。");
        }

        if (!this.ModelState.IsValid)
            return this.Page();


        (string phoneticSurname, string phoneticGivenName) = pinyinConverter.Convert(this.Input.Surname, this.Input.GivenName);
        var chinesePersonName = new ChinesePersonName(this.Input.Surname, this.Input.GivenName, phoneticSurname, phoneticGivenName);

        var personName = new PersonNameInfo(chinesePersonName.FullName, this.Input.Surname, this.Input.GivenName);

        builder
            .SetUserName(this.UserName)
            .SetPersonName(personName);
        if (this.Email != null)
            builder.SetEmail(this.Email);


        var person = builder.Build();

        person.DateOfBirth = this.Input.DateOfBirth.HasValue
            ? DateOnly.FromDateTime(this.Input.DateOfBirth.Value)
            : null;
        person.Gender = this.Input.Gender;
        person.PhoneticSurname = chinesePersonName.PhoneticSurname;
        person.PhoneticGivenName = chinesePersonName.PhoneticGivenName;
        person.PersonName.SearchHint = $"{chinesePersonName.PhoneticSurname}{chinesePersonName.GivenName}";


        this.Result = await manager.CreateAsync(person);

        if (this.Result.Succeeded)
            return this.RedirectToPage("../Detail/Index", new { anchor = person.Id });

        return this.Page();
    }

    /// <summary>
    /// 检查移动电话的有效性和唯一性。
    /// </summary>
    /// <param name="mobile"></param>
    /// <returns></returns>
    public IActionResult OnPostCheckMobile(string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
            return new JsonResult(true);

        if (!MobilePhoneNumber.TryParse(mobile, out MobilePhoneNumber mobilePhoneNumber))
            return new JsonResult("移动电话号码无效");

        if (!manager.Users.Any(p => p.PhoneNumber == mobilePhoneNumber.ToString()))
        {
            return new JsonResult(true);
        }
        return new JsonResult("此移动电话已注册");
    }

    public IActionResult OnPostCheckEmail(string email)
    {
        if (manager.Users.Any(p => p.Email == email))
            return new JsonResult("Email is exists.");
        return new JsonResult(true);
    }

    public IActionResult OnPostCheckUserName(string userName)
    {
        if (manager.Users.Any(p => p.UserName == userName))
            return new JsonResult("User name is exists.");
        return new JsonResult(true);
    }
    /// <summary>
    /// 获取拼音。
    /// </summary>
    /// <returns></returns>
    public IActionResult OnGetPinyin(string surname, string givenName)
    {
        if (string.IsNullOrWhiteSpace(givenName))
            return this.Content(string.Empty);
        (string phoneticSurname, string phoneticGivenName) = pinyinConverter.Convert(surname, givenName);
        var chinesePersonName = new ChinesePersonName(surname, givenName, phoneticSurname, phoneticGivenName);
        return this.Content($"{chinesePersonName.PhoneticSurname} {chinesePersonName.PhoneticGivenName}".Trim());
    }

    public class InputModel
    {
        [Display(Name = "Surname")]
        [StringLength(10, ErrorMessage = "Validate_StringLength")]
        public string Surname { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        [Display(Name = "Given name")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Validate_StringLength")]
        public string GivenName { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        [Display(Name = "Display name", Description = "A friendly name that appears on the user interface.")]
        public string DisplayName { get; set; } = default!;

        [Display(Name = "Phonetic surname")]
        public string PhoneticSurname { get; set; } = default!;

        [Display(Name = "Phonetic given name")]
        public string PhoneticGivenName { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        [Display(Name = "Phonetic display name")]
        public string PhoneticDisplayName { get; set; } = default!;

        [Display(Name = "Gender")]
        public Gender? Gender { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
