using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdSubjects.DirectoryLogon.Tests;
internal class StubDirectoryAccountStore : IDirectoryAccountStore
{
    public IQueryable<DirectoryAccount> Accounts => throw new NotImplementedException();

    public Task CreateAsync(DirectoryAccount account)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(DirectoryAccount account)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(DirectoryAccount account)
    {
        throw new NotImplementedException();
    }
}
