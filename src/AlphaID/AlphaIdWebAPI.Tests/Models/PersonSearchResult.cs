namespace AlphaIdWebAPI.Tests.Models;

internal record PersonSearchResult(IEnumerable<PersonModel> Persons, bool More)
{
}
