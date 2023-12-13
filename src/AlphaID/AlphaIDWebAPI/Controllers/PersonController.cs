using AlphaIdWebAPI.Models;
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

    /// <summary>
    /// Init Person Controller.
    /// </summary>
    /// <param name="personManager"></param>
    public PersonController(NaturalPersonManager personManager)
    {
        this.personManager = personManager;
    }

    /// <summary>
    /// 查找某个自然人。
    /// </summary>
    /// <param name="keywords">关键词。可以通过手机号码、姓名汉字、姓名全拼</param>
    /// <returns>Return matched peoples, up to 50 records.</returns>
    [HttpGet("Search/{keywords}")]
    public async Task<ActionResult<PersonSearchResult>> SearchAsync(string keywords)
    {
        var callClientId = this.User.ClientId();
        if (callClientId == null)
            return this.Forbid("Invalid client.");

        if (string.IsNullOrWhiteSpace(keywords))
        {
            return new PersonSearchResult(Enumerable.Empty<PersonModel>());
        }

        keywords = keywords.Trim();
        if (keywords.Length < 2)
            return new PersonSearchResult(Enumerable.Empty<PersonModel>());

        if (MobilePhoneNumber.TryParse(keywords, out MobilePhoneNumber number))
        {
            var result = await this.personManager.FindByMobileAsync(number.ToString(), this.HttpContext.RequestAborted);
            if (result == null)
                return new PersonSearchResult(Enumerable.Empty<PersonModel>());

            return new PersonSearchResult(new PersonModel[] { new(result) });
        }
        //todo 根据调用者ID来生成结对SubjectId.

        var pinyinSearchSet = this.personManager.Users.Where(p => p.PersonName.SearchHint!.StartsWith(keywords)).OrderBy(p => p.PersonName.SearchHint!.Length).ThenBy(p => p.PersonName.SearchHint);
        var pinyinSearchSetCount = pinyinSearchSet.Count();
        var pinyinSearchResult = new List<NaturalPerson>(pinyinSearchSet.Take(30));

        var nameSearchSet = this.personManager.Users.Where(p => p.PersonName.FullName.StartsWith(keywords)).OrderBy(p => p.PersonName.FullName.Length).ThenBy(p => p.PersonName.FullName);
        var nameSearchSetCount = nameSearchSet.Count();
        var nameSearchResult = new List<NaturalPerson>(nameSearchSet.Take(30));

        var searchResults = pinyinSearchResult.UnionBy(nameSearchResult, p => p.Id);

        var final = new List<PersonModel>();
        foreach (var person in searchResults)
        {
            final.Add(new PersonModel(person));
        }

        return new PersonSearchResult(final, pinyinSearchSetCount > 30 || nameSearchSetCount > 30);
    }

    /// <summary>
    /// 自然人
    /// </summary>
    /// <param name="UserName"> 主体Id. </param>
    /// <param name="Name"> Name </param>
    /// <param name="PhoneticSearchHint">  </param>
    public record PersonModel(string UserName,
                              string Name,
                              string? PhoneticSearchHint)
    {

        /// <summary>
        /// 通过NaturalPerson初始化自然人。
        /// </summary>
        /// <param name="person"></param>
        public PersonModel(NaturalPerson person)
            : this(person.UserName,
                   person.PersonName.FullName,
                   person.PersonName.SearchHint)
        { }
    }


    /// <summary>
    /// 自然人搜索结果。
    /// </summary>
    /// <param name="Persons">自然人搜索的结果。</param>
    /// <param name="More">指示一个值，表示该结果不完全，需要尝试更多的关键字来缩小搜索范围。</param>
    public record PersonSearchResult(IEnumerable<PersonModel> Persons, bool More = false);

}
