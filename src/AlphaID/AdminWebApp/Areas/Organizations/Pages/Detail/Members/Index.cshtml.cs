using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.Organizations.Pages.Detail.Members;

public class IndexModel(OrganizationManager manager, NaturalPersonManager personManager, OrganizationMemberManager memberManager) : PageModel
{
    public GenericOrganization Organization { get; set; } = default!;

    public IEnumerable<OrganizationMember> Members { get; set; } = default!;


    [BindProperty]
    [Display(Name = "Department")]
    public string? Department { get; set; }

    [BindProperty]
    [Display(Name = "Title")]
    public string? Title { get; set; }

    [BindProperty]
    [Display(Name = "Remark")]
    public string? Remark { get; set; }

    [Required(ErrorMessage = "Validate_Required")]
    [BindProperty]
    public string UserName { get; set; } = default!;

    [BindProperty]
    [Display(Name = "MembershipVisibility")]
    public MembershipVisibility Visibility { get; set; } = MembershipVisibility.Public;

    public IdOperationResult? Result { get; set; }

    public async Task<IActionResult> OnGetAsync(string anchor)
    {
        var org = await manager.FindByIdAsync(anchor);
        if (org == null)
            return NotFound();
        Organization = org;
        Members = await memberManager.GetMembersAsync(org);

        return Page();
    }

    public async Task<IActionResult> OnPostAddMemberAsync(string anchor)
    {
        var org = await manager.FindByIdAsync(anchor);
        if (org == null)
            return NotFound();
        Organization = org;
        Members = await memberManager.GetMembersAsync(org);

        var person = await personManager.FindByNameAsync(UserName);
        if (person == null)
        {
            ModelState.AddModelError(nameof(UserName), "�Ҳ�����Ա");
            return Page();
        }

        var member = new OrganizationMember(Organization, person)
        {
            Title = Title,
            Department = Department,
            Remark = Remark,
            Visibility = Visibility,
        };

        var result = await memberManager.CreateAsync(member);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return Page();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveMemberAsync(string anchor, string personId)
    {
        var org = await manager.FindByIdAsync(anchor);
        if (org == null)
            return NotFound();
        Organization = org;
        Members = await memberManager.GetMembersAsync(org);

        var member = Members.FirstOrDefault(m => m.PersonId == personId);
        if (member == null)
            return Page();

        Result = await memberManager.LeaveOrganizationAsync(member);
        if (Result.Succeeded)
        {
            Members = await memberManager.GetMembersAsync(org);
        }
        return Page();
    }

}
