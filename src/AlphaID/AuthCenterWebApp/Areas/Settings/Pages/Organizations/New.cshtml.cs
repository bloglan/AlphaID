using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Transactions;
using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.Settings.Pages.Organizations;

public class NewModel(
    OrganizationManager organizationManager,
    OrganizationMemberManager memberManager,
    ApplicationUserManager personManager) : PageModel
{
    [BindProperty]
    [Display(Name = "Name", Description = "Full name of organization.")]
    [Required(ErrorMessage = "Validate_Required")]
    [PageRemote(AdditionalFields = "__RequestVerificationToken", PageHandler = "CheckName", HttpMethod = "Post",
        ErrorMessage = "Organization name exists.")]
    public string Name { get; set; } = null!;

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Name = Name.Trim().Trim('\r', '\n').Replace(" ", string.Empty);
        using var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        if (organizationManager.Organizations.Any(p => p.Name == Name))
            ModelState.AddModelError(nameof(Name), "Organization already exists.");

        if (!ModelState.IsValid)
            return Page();

        var organization = new IdSubjects.Organization(Name)
        {
            Domicile = Input.Domicile,
            Representative = Input.Representative
        };

        IdOperationResult result = await organizationManager.CreateAsync(organization);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.Errors.Aggregate((x, y) => $"{x}, {y}"));
            return Page();
        }

        //Add current person as owner.
        ApplicationUser? person = await personManager.GetUserAsync(User);
        Debug.Assert(person != null);

        var member = new OrganizationMember(organization, person)
        {
            Title = Input.Title,
            Department = Input.Department,
            Remark = Input.Remark,
            IsOwner = true
        };
        IdOperationResult joinOrgResult = await memberManager.CreateAsync(member);
        if (!joinOrgResult.Succeeded)
        {
            ModelState.AddModelError("", joinOrgResult.Errors.Aggregate((x, y) => $"{x}, {y}"));
            return Page();
        }

        trans.Complete();

        return RedirectToPage("Index");
    }

    public IActionResult OnPostCheckName(string name)
    {
        return organizationManager.FindByName(name).Any()
            ? new JsonResult("Organization name exists.")
            : new JsonResult(true);
    }

    public class InputModel
    {
        [Display(Name = "Domicile")]
        [StringLength(100, ErrorMessage = "Validate_StringLength")]
        public string? Domicile { get; set; }

        [Display(Name = "Representative")]
        [StringLength(20, ErrorMessage = "Validate_StringLength")]
        public string? Representative { get; set; }

        [Display(Name = "Title")]
        [StringLength(50, ErrorMessage = "Validate_StringLength")]
        public string? Title { get; set; }

        [Display(Name = "Department")]
        [StringLength(50, ErrorMessage = "Validate_StringLength")]
        public string? Department { get; set; }

        [Display(Name = "Remark")]
        [StringLength(50, ErrorMessage = "Validate_StringLength")]
        public string? Remark { get; set; }
    }
}