using IdSubjects.DirectoryLogon;

namespace AlphaId.DirectoryLogon.EntityFramework;

public class QueryableLogonAccountStore : IQueryableLogonAccountStore
{
    private readonly DirectoryLogonDbContext dbContext;

    public QueryableLogonAccountStore(DirectoryLogonDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<DirectoryAccount> LogonAccounts => this.dbContext.LogonAccounts;

    public async Task<DirectoryAccount?> FindByLogonIdAsync(string logonId)
    {
        return await this.dbContext.LogonAccounts.FindAsync(logonId);
    }
}
