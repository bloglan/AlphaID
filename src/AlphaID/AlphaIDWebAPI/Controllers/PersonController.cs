using AlphaIdPlatform;
using AlphaIdPlatform.Security;
using IdSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaIdWebAPI.Controllers;

/// <summary>
/// 与自然人有关的查询。
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PersonController : ControllerBase
{
    private readonly NaturalPersonManager personManager;
    private readonly OrganizationMemberManager memberManager;
    private readonly SystemUrlInfo urlInfo;

    /// <summary>
    /// Init Person Controller.
    /// </summary>
    /// <param name="personManager"></param>
    /// <param name="urlInfo"></param>
    /// <param name="memberManager"></param>
    public PersonController(NaturalPersonManager personManager, IOptions<SystemUrlInfo> urlInfo, OrganizationMemberManager memberManager)
    {
        this.personManager = personManager;
        this.memberManager = memberManager;
        this.urlInfo = urlInfo.Value;
    }

    /// <summary>
    /// 通过 UserName 获取指定用户的信息。
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet("{userName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonInfoModel>> GetUserInfoAsync(string userName)
    {
        var person = await this.personManager.FindByNameAsync(userName);
        if (person == null)
            return this.NotFound();

        return new PersonInfoModel(person.Id, person.PersonName.FullName, person.PersonName.SearchHint,
            new Uri(this.urlInfo.AuthCenterUrl, $"/People/{person.Id}/Avatar").ToString());
    }


    /// <summary>
    /// 查找某个自然人。
    /// </summary>
    /// <param name="q">关键词。关键词非空长度必须大于2个字符时，才会返回可用结果。可以通过用户名、全名来查找自然人。</param>
    /// <returns>Return matched peoples, up to 50 records.</returns>
    [HttpGet("Suggestions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IEnumerable<SuggestedPersonModel> SearchAsync(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Enumerable.Empty<SuggestedPersonModel>();
        }

        q = q.Trim();
        if (q.Length < 2)
            return Enumerable.Empty<SuggestedPersonModel>();

        HashSet<NaturalPerson> set = new();

        if (q.Length >= 3)
        {
            var pinyinSearchSet = this.personManager.Users
                .Where(p => p.PersonName.SearchHint!.StartsWith(q))
                .OrderBy(p => p.PersonName.SearchHint!.Length)
                .ThenBy(p => p.PersonName.SearchHint)
                .Take(10).ToHashSet();
            set.UnionWith(pinyinSearchSet);
        }

        if (q.Length >= 4)
        {
            var userNameSearchSet = this.personManager.Users
                .Where(p => p.UserName.StartsWith(q))
                .OrderBy(p => p.UserName.Length)
                .ThenBy(p => p.UserName)
                .Take(10).ToHashSet();
            set.UnionWith(userNameSearchSet);
        }

        var nameSearchSet = this.personManager.Users
            .Where(p => p.PersonName.FullName.StartsWith(q))
            .OrderBy(p => p.PersonName.FullName.Length)
            .ThenBy(p => p.PersonName.FullName)
            .Take(10).ToHashSet();
        set.UnionWith(nameSearchSet);


        return set.Select(p => new SuggestedPersonModel(p)
        {
            AvatarUrl = new Uri(this.urlInfo.AuthCenterUrl, $"/People/{p.Id}/Avatar").ToString(),
        });
    }

    /// <summary>
    /// 获取指定用户的组织成员身份。
    /// </summary>
    /// <param name="userName">用户名。</param>
    /// <returns></returns>
    [HttpGet("{userName}/Memberships")]
    public async Task<ActionResult<IEnumerable<MembershipModel>>> GetMemberships(string userName)
    {
        NaturalPerson? visitor = default;
        var visitorSubjectId = this.User.SubjectId();
        if (visitorSubjectId != null)
            visitor = await this.personManager.FindByIdAsync(this.User.SubjectId()!);

        var person = await this.personManager.FindByNameAsync(userName);
        if (person == null)
            return new ActionResult<IEnumerable<MembershipModel>>(Enumerable.Empty<MembershipModel>());

        var members = this.memberManager.GetVisibleMembersOf(person, visitor);
        return new ActionResult<IEnumerable<MembershipModel>>(members.Select(m => new MembershipModel(m)));
    }

    /// <summary>
    /// 用户信息。
    /// </summary>
    /// <param name="SubjectId">Subject Id</param>
    /// <param name="Name">全名</param>
    /// <param name="SearchHint">搜索提示</param>
    /// <param name="AvatarUrl">头像Url</param>
    public record PersonInfoModel(string SubjectId,
        string Name,
        string? SearchHint,
        string? AvatarUrl);

    /// <summary>
    /// 自然人
    /// </summary>
    /// <param name="UserName">用户名</param>
    /// <param name="Name">全名</param>
    /// <param name="AvatarUrl"></param>
    public record SuggestedPersonModel(string UserName,
                              string Name,
                              string? AvatarUrl = null)
    {

        /// <summary>
        /// 通过NaturalPerson初始化自然人。
        /// </summary>
        /// <param name="person"></param>
        public SuggestedPersonModel(NaturalPerson person)
            : this(person.UserName,
                   person.PersonName.FullName)
        { }
    }

    /// <summary>
    /// 组织的成员。
    /// </summary>
    /// <param name="Department">部门</param>
    /// <param name="Title">职务</param>
    /// <param name="OrganizationId">组织标识符</param>
    /// <param name="OrganizationName">组织名称</param>
    /// <param name="Remark">备注</param>
    public record MembershipModel(
        string OrganizationId,
        string OrganizationName,
        string? Title,
        string? Department,
        string? Remark)
    {
        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="member"></param>
        public MembershipModel(OrganizationMember member)
            : this(member.OrganizationId,
                member.Organization.Name,
                member.Title,
                member.Department,
                member.Remark)
        { }
    }

}
