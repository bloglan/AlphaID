using IdSubjects;

namespace AlphaIdWebAPI.Models;



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
    /// 
    /// </summary>
    /// <param name="person"></param>
    public PersonModel(NaturalPerson person)
        : this(person.UserName,
               person.PersonName.FullName,
               person.PersonName.SearchHint)
    { }
}
