﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace AdminWebAppTests;
public class AdminWebAppFactory : WebApplicationFactory<AdminWebApp.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            //services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            //{
            //    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://localhost:49726/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever(), AuthCenterWebAppFactory.Instance.CreateClient());
            //});
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "TestScheme";
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });

    }
}
