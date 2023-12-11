using Microsoft.Extensions.DependencyInjection;

namespace IdSubjects.DirectoryLogon.Tests;

[Collection(nameof(ServiceProviderCollection))]
public class DirectoryServiceManagerTest
{
    readonly ServiceProviderFixture serviceProvider;

    public DirectoryServiceManagerTest(ServiceProviderFixture serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    [Fact(Skip = "不具备可测试性")]
    public async void CreateDirectoryService()
    {
        var store = new StubDirectoryServiceDescriptorStore();
        using var scope = this.serviceProvider.ScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<DirectoryServiceManager>();

        var directoryService = new DirectoryServiceDescriptor()
        {
            ServerAddress = "localhost",
            RootDn = "DC=qjyc,DC=cn",
        };
        var result = await manager.CreateAsync(directoryService);
        Assert.True(result.Succeeded);
    }
}