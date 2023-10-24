using IDSubjects;
using IDSubjects.Subjects;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminWebApp.Areas.Organizations.Pages;

[BindProperties]
public class NewModel : PageModel
{
    private readonly OrganizationManager manager;
    private readonly IQueryableOrganizationStore organizationStore;

    public NewModel(OrganizationManager manager, IQueryableOrganizationStore organizationStore)
    {
        this.manager = manager;
        this.organizationStore = organizationStore;
    }

    [Display(Name = "Unified social credit code")]
    public string? USCI { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "Validate_Required")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Validate_Required")]
    [Display(Name = "Register with same name anyway")]
    public bool RegisterWithSameNameAnyway { get; set; }

    [Display(Name = "Domicile")]
    public string? Domicile { get; set; }

    [Display(Name = "Contact")]
    public string? Contact { get; set; }

    [Display(Name = "Legal person name")]
    public string? LegalPersonName { get; set; }

    [Display(Name = "Established at")]
    [DataType(DataType.Date)]
    public DateTime? EstablishedAt { get; set; }

    [Display(Name = "Term begin")]
    [DataType(DataType.Date)]
    public DateTime? TermBegin { get; set; }

    [Display(Name = "Term end", Prompt = "dd", Description = "weew")]
    [DataType(DataType.Date)]
    public DateTime? TermEnd { get; set; }

    [Display(Name = "Creator ID")]
    public string? CreatorId { get; set; }


    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(this.USCI))
        {
            if (!USCC.TryParse(this.USCI, out USCC uscc))
                this.ModelState.AddModelError(nameof(this.USCI), "ͳһ������ô��벻��ȷ��");

            var usciExists = await this.organizationStore.FindByIdentityAsync("ͳһ������ô���", uscc.ToString());
            if (usciExists != null)
                this.ModelState.AddModelError(nameof(this.USCI), "ͳһ������ô����ѱ��Ǽǡ�");
        }

        if (!this.ModelState.IsValid)
            return this.Page();

        var nameExists = await this.manager.SearchByNameAsync(this.Name);
        if (nameExists.Any())
        {
            if (!this.RegisterWithSameNameAnyway)
            {
                this.ModelState.AddModelError(nameof(this.Name), "���д���ͬ������֯�����ȷʵҪע�ᣬ�빴ѡ����ʹ������ͬ��ҲҪע�ᡱ��ѡ��");
                return this.Page();
            }
        }

        var factory = new OrganizationBuilder(this.Name);
        if (!string.IsNullOrWhiteSpace(this.USCI))
        {
            if (USCC.TryParse(this.USCI, out USCC uscc))
            {
                factory.SetUSCI(uscc);
            }
            else
            {
                this.ModelState.AddModelError(nameof(this.USCI), "ͳһ������ô��벻��ȷ��");
            }
        }

        var org = factory.Organization;
        org.Domicile = this.Domicile;
        org.LegalPersonName = this.LegalPersonName;
        org.EstablishedAt = this.EstablishedAt;
        org.TermBegin = this.TermBegin;
        org.TermEnd = this.TermEnd;

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            var result = await this.manager.CreateAsync(org);
            return this.RedirectToPage("Detail/Index", new { id = org.Id });
        }
        catch (Exception ex)
        {
            this.ModelState.AddModelError("", ex.Message);
            return this.Page();
        }
    }
}