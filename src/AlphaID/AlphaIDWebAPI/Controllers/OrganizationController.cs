using AlphaIdWebAPI.Models;
using IdSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaIdWebAPI.Controllers;

/// <summary>
/// 组织信息。
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationStore organizationStore;
    private readonly OrganizationMemberManager memberManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationStore"></param>
    /// <param name="memberManager"></param>
    public OrganizationController(IOrganizationStore organizationStore, OrganizationMemberManager memberManager)
    {
        this.organizationStore = organizationStore;
        this.memberManager = memberManager;
    }

    /// <summary>
    /// 获取组织信息。
    /// </summary>
    /// <param name="id">组织的SubjectId</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationModel>> GetAsync(string id)
    {
        var org = await this.organizationStore.FindByIdAsync(id);
        return org == null ? this.NotFound() : new OrganizationModel(org);
    }

    /// <summary>
    /// 获取组织的成员。
    /// </summary>
    /// <param name="id">组织的SubjectId</param>
    /// <returns></returns>
    [HttpGet("{id}/Members")]
    public async Task<IEnumerable<MembershipModel>> GetMembersAsync(string id)
    {
        var org = await this.organizationStore.FindByIdAsync(id);
        if (org == null)
            return Enumerable.Empty<MembershipModel>();
        var members = await this.memberManager.GetMembersAsync(org);

        return from member in members select new MembershipModel(member);
    }

    /// <summary>
    /// 给定关键字查找组织。
    /// </summary>
    /// <remarks>
    /// 支持通过登记的统一社会信用代码、组织机构代码、组织名称的一部分进行查找。
    /// </remarks>
    /// <param name="keywords">关键字</param>
    /// <returns></returns>
    [HttpGet("Search/{keywords}")]
    [AllowAnonymous]
    public OrganizationSearchResult Search(string keywords)
    {
        var searchResults = this.organizationStore.Organizations.Where(p => p.Name.Contains(keywords) && p.Enabled);

        var result = new OrganizationSearchResult(searchResults.Take(50).Select(p => new OrganizationModel(p)), searchResults.Count() > 50);
        return result;
    }

    /// <summary>
    /// GenericOrganization.
    /// </summary>
    /// <param name="SubjectId">Id</param>
    /// <param name="Name">名称。</param>
    /// <param name="Domicile">住所。</param>
    /// <param name="Contact">联系方式。</param>
    /// <param name="LegalPersonName">组织的负责人或代表人名称。</param>
    /// <param name="Expires">有效期。</param>
    public record OrganizationModel(string SubjectId,
                                    string Name,
                                    string? Domicile,
                                    string? Contact,
                                    string? LegalPersonName,
                                    DateOnly? Expires)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        public OrganizationModel(GenericOrganization organization)
            : this(organization.Id,
                   organization.Name,
                   organization.Domicile,
                   organization.Contact,
                   organization.Representative,
                   organization.TermEnd)
        { }

    }

    /// <summary>
    /// 组织机构概要
    /// </summary>
    /// <param name="Organizations"> 此查找结果包含的组织信息。 </param>
    /// <param name="More"> 指示出组织信息集合外，是否还有更多结果未返回。这意味着关键字所匹配结果集较大，需要重新选择关键字以便缩小匹配范围。 </param>
    public record OrganizationSearchResult(IEnumerable<OrganizationModel> Organizations, bool More = false);

}
