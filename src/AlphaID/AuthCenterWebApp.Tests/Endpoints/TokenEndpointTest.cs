﻿using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AuthCenterWebApp.Tests.Endpoints;

[Collection(nameof(TestServerCollection))]
public class TokenEndpointTest(AuthCenterWebAppFactory factory)
{
    [Fact]
    public async Task GrantByClientCredentials()
    {
        var client = factory.CreateClient();
        var forms = new Dictionary<string, string>()
        {
            { "client_id", "d70700eb-c4d8-4742-a79a-6ecf2064b27c" },
            { "client_secret", "i7zcwJu)5pgIA()huJWRoT@oCLHpwfe^" },
            { "grant_type", "client_credentials" },
        };
        var response = await client.PostAsync("/connect/token", new FormUrlEncodedContent(forms));
        response.EnsureSuccessStatusCode();
        var tokenData = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.Equal("Bearer", tokenData!.TokenType);
    }

    [Fact]
    public async Task GrantByPasswordOwner()
    {
        var client = factory.CreateClient();
        var forms = new Dictionary<string, string>()
        {
            { "client_id", "d70700eb-c4d8-4742-a79a-6ecf2064b27c" },
            { "client_secret", "i7zcwJu)5pgIA()huJWRoT@oCLHpwfe^" },
            { "username", "liubei" },
            { "password", "Pass123$" },
            { "grant_type", "password" },
        };
        var response = await client.PostAsync("/connect/token", new FormUrlEncodedContent(forms));
        response.EnsureSuccessStatusCode();
        var tokenData = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.Equal("Bearer", tokenData!.TokenType);
    }

    public record TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = default!;
    }
}
