using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.SystemSettings.Pages.DirectoryService
{
    public class IndexModel : PageModel
    {
        DirectoryServiceManager directoryServiceManager;

        public IndexModel(DirectoryServiceManager directoryServiceManager)
        {
            this.directoryServiceManager = directoryServiceManager;
        }

        public IdSubjects.DirectoryLogon.DirectoryServiceDescriptor Data { get; set; } = default!;

        public async Task<IActionResult> OnGet(int anchor)
        {
            var ds = await this.directoryServiceManager.FindByIdAsync(anchor);
            if (ds == null)
                return this.NotFound();
            this.Data = ds;
            return this.Page();
        }
    }
}
