﻿using IdSubjects.DirectoryLogon;

namespace AlphaId.DirectoryLogon.EntityFramework;

public class DirectoryAccountStore(DirectoryLogonDbContext dbContext) : IDirectoryAccountStore
{
    public IQueryable<DirectoryAccount> Accounts => dbContext.LogonAccounts;

    public async Task CreateAsync(DirectoryAccount account)
    {
        dbContext.LogonAccounts.Add(account);
        _ = await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DirectoryAccount account)
    {
        dbContext.LogonAccounts.Remove(account);
        _ = await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DirectoryAccount account)
    {
        dbContext.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _ = await dbContext.SaveChangesAsync();
    }
}
