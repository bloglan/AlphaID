using AlphaIdPlatform.Helpers;
using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.People.Pages.Detail.Membership
{
    public class OfModel(OrganizationMemberManager memberManager, NaturalPersonManager naturalPersonManager) : PageModel
    {
        public OrganizationMember Member { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IdOperationResult? OperationResult { get; set; }

        public IEnumerable<SelectListItem> MembershipVisibilities { get; set; } = EnumHelper.GetSelectListItems<MembershipVisibility>();

        public async Task<IActionResult> OnGetAsync(string anchor, string orgId)
        {
            var person = await naturalPersonManager.FindByIdAsync(anchor);
            if (person == null)
                return this.NotFound();
            var members = await memberManager.GetMembersOfAsync(person);
            var member = members.FirstOrDefault(p => p.OrganizationId == orgId);
            if (member == null)
                return this.NotFound();
            this.Member = member;
            this.Input = new InputModel
            {
                Title = member.Title,
                Department = member.Department,
                Remark = member.Remark,
                IsOwner = member.IsOwner,
                Visibility = member.Visibility,
            };
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string anchor, string orgId)
        {
            var person = await naturalPersonManager.FindByIdAsync(anchor);
            if (person == null)
                return this.NotFound();
            var members = await memberManager.GetMembersOfAsync(person);
            var member = members.FirstOrDefault(p => p.OrganizationId == orgId);
            if (member == null)
                return this.NotFound();
            this.Member = member;

            member.Title = this.Input.Title;
            member.Department = this.Input.Department;
            member.Remark = this.Input.Remark;
            member.IsOwner = this.Input.IsOwner;
            member.Visibility = this.Input.Visibility;

            this.OperationResult = await memberManager.UpdateAsync(member);

            return this.Page();
        }

        public class InputModel
        {
            [Display(Name = "Title")]
            [StringLength(50, ErrorMessage = "Validate_StringLength")]
            public string? Title { get; set; }

            [Display(Name = "Department")]
            [StringLength(50, ErrorMessage = "Validate_StringLength")]
            public string? Department { get; set; }

            [Display(Name = "Remark")]
            [StringLength(50, ErrorMessage = "Validate_StringLength")]
            public string? Remark { get; set; }

            [Display(Name = "Is owner", Description = "The owner of organization can fully mange organization by themselves.")]
            public bool IsOwner { get; set; }

            [Display(Name = "Membership visibility")]
            public MembershipVisibility Visibility { get; set; }
        }
    }
}
