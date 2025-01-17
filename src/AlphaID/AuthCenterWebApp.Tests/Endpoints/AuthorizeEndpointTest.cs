using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AuthCenterWebApp.Tests.Endpoints;

[Collection(nameof(TestServerCollection))]
public class AuthorizeEndpointTest(AuthCenterWebAppFactory factory)
{
    [Fact(Skip = "不再考虑此测试")]
    public async Task GotoAuthenticationPage()
    {
        HttpClient client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        HttpResponseMessage response = await client.GetAsync("/connect/authorize");
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
    }
}