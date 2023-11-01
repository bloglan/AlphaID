﻿using IDSubjects;
using Microsoft.EntityFrameworkCore;

namespace AlphaID.EntityFramework;

public class OrganizationMemberStore : IOrganizationMemberStore
{
    private readonly IDSubjectsDbContext dbContext;

    public OrganizationMemberStore(IDSubjectsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<OrganizationMember> OrganizationMembers => this.dbContext.OrganizationMembers.Include(p => p.Organization).Include(p => p.Person);

    public async Task<OrganizationMember?> FindAsync(string personId, string organizationId)
    {
        return await this.dbContext.OrganizationMembers.FirstOrDefaultAsync(p => p.PersonId == personId && p.OrganizationId == organizationId);
    }

    public async Task<IdOperationResult> CreateAsync(OrganizationMember item)
    {
        this.dbContext.OrganizationMembers.Add(item);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }

    public async Task<IdOperationResult> DeleteAsync(OrganizationMember item)
    {
        this.dbContext.OrganizationMembers.Remove(item);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }

    public async Task<IdOperationResult> UpdateAsync(OrganizationMember item)
    {
        this.dbContext.OrganizationMembers.Update(item);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }
}
