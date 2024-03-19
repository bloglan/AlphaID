﻿namespace AuthCenterWebApp.Tests.Endpoints;

[Collection(nameof(TestServerCollection))]
public class DiscoveryDocumentTest(AuthCenterWebAppFactory factory)
{
    [Fact]
    public async Task DocumentOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/.well-known/openid-configuration");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}
