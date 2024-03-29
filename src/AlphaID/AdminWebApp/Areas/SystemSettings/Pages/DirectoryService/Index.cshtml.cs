using IdSubjects.DirectoryLogon;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.SystemSettings.Pages.DirectoryService
{
    public class IndexModel(DirectoryServiceManager directoryServiceManager) : PageModel
    {
        public DirectoryServiceDescriptor Data { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int anchor)
        {
            var ds = await directoryServiceManager.FindByIdAsync(anchor);
            if (ds == null)
                return this.NotFound();
            this.Data = ds;
            return this.Page();
        }
    }
}
