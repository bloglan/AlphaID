using AlphaIdWebAPI.Tests.Models;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace AlphaIdWebAPI.Tests.Controllers;

[Collection(nameof(TestServerCollection))]
public class PersonControllerTests
{
    private readonly AlphaIdApiFactory factory;

    public PersonControllerTests(AlphaIdApiFactory factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData("刘备")]
    [InlineData("liubei")]
    public async Task SearchPerson(string keywords)
    {
        var client = this.factory.CreateAuthenticatedClient();

        var response = await client.GetAsync($"/api/Person/Search/{WebUtility.UrlEncode(keywords)}");
        _ = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<PersonSearchResult>();
        Assert.True(data!.Persons.Any());
    }

    internal record PersonModel(string UserName, string Name, string? PhoneticSearchHint)
    {
    }


    internal record PersonSearchResult(IEnumerable<PersonModel> Persons, bool More)
    {
    }

}
