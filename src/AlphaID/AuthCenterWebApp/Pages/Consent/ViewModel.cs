namespace AuthCenterWebApp.Pages.Consent;

public class ViewModel
{
    public string ClientName { get; set; } = default!;
    public string? ClientUrl { get; set; }
    public string? ClientLogoUrl { get; set; }
    public bool AllowRememberConsent { get; set; }

    public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = default!;
    public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = default!;
}

public class ScopeViewModel
{
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Description { get; set; }
    public bool Emphasize { get; set; }
    public bool Required { get; set; }
    public bool Checked { get; set; }
    public IEnumerable<ResourceViewModel> Resources { get; set; } = [];
}

public class ResourceViewModel
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}