﻿using IntegrationTestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IdSubjects.Tests;

[Collection(nameof(ServiceProviderCollection))]
public class NaturalPersonManagerTest(ServiceProviderFixture serviceProvider)
{
    private readonly NaturalPerson _person = new("zhangsan", new PersonNameInfo("张三"));

    [Fact]
    public async Task SetTimeZone()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        await manager.CreateAsync(_person);

        //using IANA time zone name
        var result = await manager.SetTimeZone(_person, "Asia/Shanghai");
        Assert.True(result.Succeeded);
        Assert.Equal("Asia/Shanghai", _person.TimeZone);

        //using Microsoft time zone name
        result = await manager.SetTimeZone(_person, "China Standard Time");
        Assert.True(result.Succeeded);
        Assert.Equal("Asia/Shanghai", _person.TimeZone);
    }

    [Fact]
    public async Task CreateWithPassword()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();

        var now = DateTimeOffset.UtcNow;
        manager.TimeProvider = new FrozenTimeProvider(now);
        var result = await manager.CreateAsync(_person, "Pass123$");

        Assert.True(result.Succeeded);
        Assert.NotNull(_person.PasswordHash);
        Assert.Equal(now, _person.PasswordLastSet!.Value);
        Assert.Equal(now, _person.WhenCreated);
        Assert.Equal(now, _person.WhenChanged);
        Assert.Equal(now, _person.PersonWhenChanged);
    }

    [Fact]
    public async Task CreateWithoutPassword()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        
        var now = DateTimeOffset.UtcNow;
        manager.TimeProvider = new FrozenTimeProvider(now);

        var result = await manager.CreateAsync(_person);

        Assert.True(result.Succeeded);
        Assert.False(_person.PasswordLastSet.HasValue);
        Assert.Equal(now, _person.WhenCreated);
        Assert.Equal(now, _person.WhenChanged);
        Assert.Equal(now, _person.PersonWhenChanged);
    }

    [Fact]
    public async Task SetUpdateTimeWhenUpdate()
    {

        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        await manager.CreateAsync(_person);

        var utcNow = new DateTimeOffset(2023, 11, 4, 3, 50, 34, TimeSpan.Zero);
        manager.TimeProvider = new FrozenTimeProvider(utcNow);

        await manager.UpdateAsync(_person);

        Assert.Equal(utcNow, _person.WhenChanged);
    }

    [Fact]
    public async Task AddPasswordWillSetPasswordLastSetTime()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        await manager.CreateAsync(_person);

        var result = await manager.AddPasswordAsync(_person, "Password$1");
        Assert.True(result.Succeeded);
        Assert.NotNull(_person.PasswordLastSet);
    }

    [Fact]
    public async Task RemovePassword()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();

        await manager.CreateAsync(_person, "Pass123$");

        var result = await manager.RemovePasswordAsync(_person);
        Assert.True(result.Succeeded);
        Assert.Null(_person.PasswordLastSet);
    }

    [Fact]
    public async Task ChangePassword()
    {
        using var scope = serviceProvider.ServiceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        manager.Options.Password.RememberPasswordHistory = 1;

        await manager.CreateAsync(_person, "Pass123$");

        var result = await manager.ChangePasswordAsync(_person, "Pass123$", "Pass1234$");
        Assert.True(result.Succeeded);

        var passwordHistoryStore = scope.ServiceProvider.GetRequiredService<IPasswordHistoryStore>();
        var passwords = passwordHistoryStore.GetPasswords(_person, 10);
        Assert.Single(passwords);

        //change password again with same old password will fail.
        result = await manager.ChangePasswordAsync(_person, "Pass1234$", "Pass1234$");
        Assert.False(result.Succeeded);
    }
}