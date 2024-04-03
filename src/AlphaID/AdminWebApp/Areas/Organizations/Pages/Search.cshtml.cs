using IdSubjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AdminWebApp.Areas.Organizations.Pages;

public class SearchModel(OrganizationSearcher searcher) : PageModel
{
    public IEnumerable<GenericOrganization> Results { get; set; } = [];

    public IActionResult OnGet()
    {
        StringValues q = Request.Query["q"];
        if (string.IsNullOrWhiteSpace(q))
            return Page();

        Results = searcher.Search(q!);
        return Page();
    }
}