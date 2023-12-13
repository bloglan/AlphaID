using System.Net;
using Xunit;

namespace AlphaIdWebAPI.Tests.Controllers.Oidc;

[Collection(nameof(TestServerCollection))]
public class ClientControllerTests
{
    private readonly AlphaIdApiFactory factory;

    public ClientControllerTests(AlphaIdApiFactory factory)
    {
        this.factory = factory;
    }


    [Fact]
    public async Task GetClientName()
    {
        var client = this.factory.CreateAuthenticatedClient();

        var response = await client.GetAsync($"api/Oidc/Client/d70700eb-c4d8-4742-a79a-6ecf2064b27c");
        response.EnsureSuccessStatusCode();
        Assert.Equal("Alpha ID Management Center", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetNonExistsClientName()
    {
        var client = this.factory.CreateAuthenticatedClient();

        var response = await client.GetAsync($"api/Oidc/Client/non-exists-client-id");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
