using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IdSubjects.RealName.Tests;
public class RealNameManagerTest : IClassFixture<ServiceProviderFixture>
{
    private readonly ServiceProviderFixture serviceProvider;

    public RealNameManagerTest(ServiceProviderFixture serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    [Fact]
    public async Task AddAuthentication()
    {
        var person = new NaturalPerson("zhangsan", new PersonNameInfo("张小三"));
        var authentication = new DocumentedRealNameAuthentication(
            new ChineseIdCardDocument()
            {
                Address = "Address",
                CardNumber = "370686193704095897",
                DateOfBirth = new(1937, 4, 9),
                Ethnicity = "汉",
                IssueDate = new(2000, 1, 1),
                Name = "张三",
                Issuer = "Issuer",
                Sex = Sex.Male,
            },
            new("张三", "张", "三"),
            DateTimeOffset.UtcNow,
            "Test validator");

        using var scope = this.serviceProvider.ScopeFactory.CreateScope();
        var personManager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        await personManager.CreateAsync(person);

        //Test
        var realManager = scope.ServiceProvider.GetRequiredService<RealNameManager>();
        var result = await realManager.AuthenticateAsync(person, authentication);

        Assert.True(result.Succeeded);
        Assert.Equal("张三", person.PersonName.FullName);
    }

    [Fact]
    public async Task CannotChangeNameWhenRealNameAuthenticationExists()
    {
        var person = new NaturalPerson("zhangsan", new PersonNameInfo("张小三"));
        

        using var scope = this.serviceProvider.ScopeFactory.CreateScope();
        var personManager = scope.ServiceProvider.GetRequiredService<NaturalPersonManager>();
        await personManager.CreateAsync(person);

        var target = (await personManager.FindByIdAsync(person.Id))!;

        var realManager = scope.ServiceProvider.GetRequiredService<RealNameManager>();
        var authentication = new DocumentedRealNameAuthentication(
            new ChineseIdCardDocument()
            {
                Address = "Address",
                CardNumber = "370686193704095897",
                DateOfBirth = new(1937, 4, 9),
                Ethnicity = "汉",
                IssueDate = new(2000, 1, 1),
                Name = "张三",
                Issuer = "Issuer",
                Sex = Sex.Male,
            },
            new("张三", "张", "三"),
            DateTimeOffset.UtcNow,
            "Test validator");
        await realManager.AuthenticateAsync(target, authentication);


        target.PersonName = new("张三三");
        var result = await personManager.UpdateAsync(target);
        Assert.False(result.Succeeded);
    }
}
