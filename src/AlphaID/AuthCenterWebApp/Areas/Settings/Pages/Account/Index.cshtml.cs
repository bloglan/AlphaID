using IdSubjects;
using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCenterWebApp.Areas.Settings.Pages.Account
{
    public class IndexModel : PageModel
    {
        private DirectoryAccountManager directoryAccountManager;
        NaturalPersonManager naturalPersonManager;

        public IndexModel(DirectoryAccountManager directoryAccountManager, NaturalPersonManager naturalPersonManager)
        {
            this.directoryAccountManager = directoryAccountManager;
            this.naturalPersonManager = naturalPersonManager;
        }

        public IEnumerable<DirectoryAccount> DirectoryAccounts { get; set; } = Enumerable.Empty<DirectoryAccount>();

        public async Task OnGet()
        {
            var person = await this.naturalPersonManager.GetUserAsync(this.User) ?? throw new InvalidOperationException("找不到用户。");
            this.DirectoryAccounts = this.directoryAccountManager.GetLogonAccounts(person);
        }
    }
}
