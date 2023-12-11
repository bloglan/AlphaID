using IdSubjects.DirectoryLogon;

namespace AlphaId.DirectoryLogon.EntityFramework;

public class DirectoryAccountStore : IDirectoryAccountStore
{
    private readonly DirectoryLogonDbContext dbContext;

    public DirectoryAccountStore(DirectoryLogonDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<DirectoryAccount> Accounts => this.dbContext.LogonAccounts;

    public async Task CreateAsync(DirectoryAccount account)
    {
        this.dbContext.LogonAccounts.Add(account);
        _ = await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DirectoryAccount account)
    {
        this.dbContext.LogonAccounts.Remove(account);
        _ = await this.dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DirectoryAccount account)
    {
        this.dbContext.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _ = await this.dbContext.SaveChangesAsync();
    }
}
