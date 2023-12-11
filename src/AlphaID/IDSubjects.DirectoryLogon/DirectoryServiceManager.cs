using Microsoft.Extensions.Logging;
using System.DirectoryServices.AccountManagement;

namespace IdSubjects.DirectoryLogon;

/// <summary>
/// Directory service manager.
/// </summary>
public class DirectoryServiceManager
{
    private readonly IDirectoryServiceDescriptorStore directoryServiceDescriptorStore;
    private readonly ILogger<DirectoryServiceManager>? logger;

    /// <summary>
    /// Init DirectoryServiceManager.
    /// </summary>
    /// <param name="directoryServiceDescriptorStore"></param>
    /// <param name="logger"></param>
    public DirectoryServiceManager(IDirectoryServiceDescriptorStore directoryServiceDescriptorStore, ILogger<DirectoryServiceManager>? logger = null)
    {
        this.directoryServiceDescriptorStore = directoryServiceDescriptorStore;
        this.logger = logger;
    }

    /// <summary>
    /// Gets list of DirectoryService.
    /// </summary>
    public IEnumerable<DirectoryServiceDescriptor> Services => this.directoryServiceDescriptorStore.Services;

    /// <summary>
    /// Create a directory service.
    /// </summary>
    /// <param name="directoryServiceDescriptor"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    public async Task<IdOperationResult> CreateAsync(DirectoryServiceDescriptor directoryServiceDescriptor)
    {
        if (!directoryServiceDescriptor.DefaultUserAccountContainer.EndsWith(directoryServiceDescriptor.RootDn))
            return IdOperationResult.Failed("默认UserContainer必须是RootDN的子集。");

        try
        {
            using PrincipalContext context = directoryServiceDescriptor.GetRootContext();

            //没有异常，说明访问成功，可以持久化DirectoryService配置。
            await this.directoryServiceDescriptorStore.CreateAsync(directoryServiceDescriptor);
            return IdOperationResult.Success;
        }
        catch (Exception)
        {
            this.logger?.LogInformation("创建目录服务时出错，测试目录服务连接没有成功。");
            return IdOperationResult.Failed("创建目录服务时出错，测试目录服务连接没有成功。");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryServiceDescriptor"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> UpdateAsync(DirectoryServiceDescriptor directoryServiceDescriptor)
    {
        if (!directoryServiceDescriptor.DefaultUserAccountContainer.EndsWith(directoryServiceDescriptor.RootDn))
            return IdOperationResult.Failed("默认UserContainer必须是RootDN的子集。");

        try
        {
            using PrincipalContext context = directoryServiceDescriptor.GetRootContext();

            //没有异常，说明访问成功，可以持久化DirectoryService配置。
            await this.directoryServiceDescriptorStore.UpdateAsync(directoryServiceDescriptor);
            return IdOperationResult.Success;
        }
        catch (Exception)
        {
            this.logger?.LogInformation("创建目录服务时出错，测试目录服务连接没有成功。");
            return IdOperationResult.Failed("创建目录服务时出错，测试目录服务连接没有成功。");
        }
    }

    /// <summary>
    /// Delete a directory service.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> DeleteAsync(DirectoryServiceDescriptor data)
    {
        await this.directoryServiceDescriptorStore.DeleteAsync(data);
        return IdOperationResult.Success;
    }

    /// <summary>
    /// Find Directory Service by Id.
    /// </summary>
    /// <param name="serviceId"></param>
    /// <returns></returns>
    public Task<DirectoryServiceDescriptor?> FindByIdAsync(int serviceId)
    {
        return this.directoryServiceDescriptorStore.FindByIdAsync(serviceId);
    }
}
