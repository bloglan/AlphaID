﻿using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace AlphaIdWebAPI.Tests.Controllers.Oidc;

[Collection(nameof(TestServerCollection))]
public class ClientControllerTests(AlphaIdApiFactory factory)
{
    [Fact]
    public async Task GetClientName()
    {
        var client = factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("api/Oidc/Client/d70700eb-c4d8-4742-a79a-6ecf2064b27c");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ClientModel>();
        Assert.Equal("Alpha ID Management Center", result!.Name);
    }

    [Fact]
    public async Task GetNonExistsClientName()
    {
        var client = factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("api/Oidc/Client/non-exists-client-id");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public record ClientModel(string Name);
}
