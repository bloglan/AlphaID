﻿using IdSubjects;
using IdSubjects.RealName.Requesting;
using Microsoft.EntityFrameworkCore;

namespace AlphaId.RealName.EntityFramework;

internal class RealNameRequestStore : IRealNameRequestStore
{
    private readonly RealNameDbContext dbContext;

    public RealNameRequestStore(RealNameDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<RealNameRequest> Requests => this.dbContext.RealNameRequests.AsNoTracking();

    public async Task<RealNameRequest?> FindByIdAsync(int id)
    {
        return await this.dbContext.RealNameRequests.FindAsync(id);
    }

    public async Task<IdOperationResult> CreateAsync(RealNameRequest request)
    {
        this.dbContext.RealNameRequests.Add(request);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }

    public async Task<IdOperationResult> UpdateAsync(RealNameRequest request)
    {
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }
}