﻿using IdSubjects.RealName.Requesting;

namespace IdSubjects.RealName.Tests;
internal class StubRealNameRequestStore : IRealNameRequestStore
{
    private readonly HashSet<RealNameRequest> set = [];

    public Task<IdOperationResult> CreateAsync(RealNameRequest request)
    {
        var lastId = this.set.OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
        request.Id = lastId + 1;
        this.set.Add(request);
        return Task.FromResult(IdOperationResult.Success);
    }

    public Task<IdOperationResult> UpdateAsync(RealNameRequest request)
    {
        //do nothing.
        return Task.FromResult(IdOperationResult.Success);
    }

    public IQueryable<RealNameRequest> Requests => this.set.AsQueryable();

    public Task<RealNameRequest?> FindByIdAsync(int id)
    {
        return Task.FromResult(this.set.FirstOrDefault(r => r.Id == id));
    }
}
