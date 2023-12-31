using AlphaIdPlatform.Security;
using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.People.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NaturalPersonManager personManager;
        private readonly OrganizationMemberManager organizationMemberManager;


        public IndexModel(NaturalPersonManager personManager, OrganizationMemberManager organizationMemberManager)
        {
            this.personManager = personManager;
            this.organizationMemberManager = organizationMemberManager;
        }

        public NaturalPerson Person { get; set; } = default!;

        public bool UserIsOwner { get; set; }

        public IEnumerable<OrganizationMember> Members { get; set; } = Enumerable.Empty<OrganizationMember>();

        public async Task<IActionResult> OnGetAsync(string anchor)
        {
            //Support both userAnchor and user ID.
            var person = await this.personManager.FindByNameAsync(anchor)
                ?? await this.personManager.FindByIdAsync(anchor);
            if (person == null)
                return this.NotFound();
            this.Person = person;

            NaturalPerson? visitor = await this.personManager.GetUserAsync(this.User);

            this.Members = await this.organizationMemberManager.GetVisibleMembersOfAsync(person, visitor);

            if (!this.User.Identity!.IsAuthenticated)
            {
                return this.Page();
            }

            if (this.User.SubjectId() == this.Person.Id)
                this.UserIsOwner = true;

            return this.Page();
        }
    }
}
