using AlphaIdPlatform;
using Castle.Components.DictionaryAdapter;
using IdSubjects;
using IdSubjects.Subjects;
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
    private SystemUrlInfo urlInfo;
    /// <summary>
    /// Init Person Controller.
    /// </summary>
    /// <param name="personManager"></param>
    public PersonController(NaturalPersonManager personManager, IOptions<SystemUrlInfo> urlInfo)
    {
        this.personManager = personManager;
        this.urlInfo = urlInfo.Value;
    }

    /// <summary>
    /// 通过 UserName 获取指定用户的信息。
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserInfoModel>> GetUserInfoAsync(string userName)
    {
        var person = await this.personManager.FindByNameAsync(userName);
        if (person == null)
            return this.NotFound();

        return new UserInfoModel(person.Id, person.PersonName.SearchHint);
    }

    public record UserInfoModel(string SubjectId, string? SearchHint)
    {
    }

    /// <summary>
    /// 查找某个自然人。
    /// </summary>
    /// <param name="keywords">关键词。关键词非空长度必须大于2个字符时，才会返回可用结果。可以通过用户名、全名来查找自然人。</param>
    /// <returns>Return matched peoples, up to 50 records.</returns>
    [HttpGet("Search/{keywords}")]
    public async Task<IEnumerable<SearchPersonModel>> SearchAsync(string keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
        {
            return Enumerable.Empty<SearchPersonModel>();
        }

        keywords = keywords.Trim();
        if (keywords.Length < 2)
            return Enumerable.Empty<SearchPersonModel>();

        HashSet<NaturalPerson> set = new();

        if (keywords.Length >= 3)
        {
            var pinyinSearchSet = this.personManager.Users
                .Where(p => p.PersonName.SearchHint!.StartsWith(keywords))
                .OrderBy(p => p.PersonName.SearchHint!.Length)
                .ThenBy(p => p.PersonName.SearchHint)
                .Take(10);
            set.UnionWith(pinyinSearchSet);
        }

        if (keywords.Length >= 4)
        {
            var userNameSearchSet = this.personManager.Users
                .Where(p => p.UserName.StartsWith(keywords))
                .OrderBy(p => p.UserName.Length)
                .ThenBy(p => p.UserName)
                .Take(10);
            set.UnionWith(userNameSearchSet);
        }

        var nameSearchSet = this.personManager.Users
            .Where(p => p.PersonName.FullName.StartsWith(keywords))
            .OrderBy(p => p.PersonName.FullName.Length)
            .ThenBy(p => p.PersonName.FullName)
            .Take(10);
        set.UnionWith(nameSearchSet);


        return set.Select(p => new SearchPersonModel(p)
        {
            Avatar = p.ProfilePicture != null ? new Uri(this.urlInfo.AuthCenterUrl, $"/People/{p.Id}/Avatar").ToString() : null,
        });
    }

    /// <summary>
    /// 自然人
    /// </summary>
    /// <param name="UserName">用户名</param>
    /// <param name="Name">全名</param>
    public record SearchPersonModel(string UserName,
                              string Name,
                              string? Avatar = null)
    {

        /// <summary>
        /// 通过NaturalPerson初始化自然人。
        /// </summary>
        /// <param name="person"></param>
        public SearchPersonModel(NaturalPerson person)
            : this(person.UserName,
                   person.PersonName.FullName)
        { }
    }
}
