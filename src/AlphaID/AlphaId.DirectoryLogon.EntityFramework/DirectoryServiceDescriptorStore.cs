using IdSubjects.DirectoryLogon;

namespace AlphaId.DirectoryLogon.EntityFramework;

public class DirectoryServiceDescriptorStore : IDirectoryServiceDescriptorStore
{
    private readonly DirectoryLogonDbContext dbContext;

    public DirectoryServiceDescriptorStore(DirectoryLogonDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<DirectoryServiceDescriptor> Services => this.dbContext.DirectoryServices;

    public async Task CreateAsync(DirectoryServiceDescriptor serviceDescriptor)
    {
        this.dbContext.DirectoryServices.Add(serviceDescriptor);
        _ = await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DirectoryServiceDescriptor serviceDescriptor)
    {
        this.dbContext.DirectoryServices.Remove(serviceDescriptor);
        _ = await this.dbContext.SaveChangesAsync();
    }

    public async Task<DirectoryServiceDescriptor?> FindByIdAsync(int id)
    {
        return await this.dbContext.DirectoryServices.FindAsync(id);
    }

    public async Task UpdateAsync(DirectoryServiceDescriptor serviceDescriptor)
    {
        this.dbContext.Entry(serviceDescriptor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _ = await this.dbContext.SaveChangesAsync();
    }
}
