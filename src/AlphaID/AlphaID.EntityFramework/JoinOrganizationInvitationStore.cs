using IdSubjects;
using IdSubjects.Invitations;
using Microsoft.EntityFrameworkCore;

namespace AlphaId.EntityFramework;
public class JoinOrganizationInvitationStore : IJoinOrganizationInvitationStore
{
    private readonly IdSubjectsDbContext dbContext;

    public JoinOrganizationInvitationStore(IdSubjectsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<JoinOrganizationInvitation> Invitations => this.dbContext.JoinOrganizationInvitations;
    public async Task<IdOperationResult> CreateAsync(JoinOrganizationInvitation invitation)
    {
        this.dbContext.JoinOrganizationInvitations.Add(invitation);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }

    public async Task<IdOperationResult> UpdateAsync(JoinOrganizationInvitation invitation)
    {
        if (this.dbContext.Entry(invitation).State == EntityState.Detached)
        {
            var origin = await this.dbContext.JoinOrganizationInvitations.FindAsync(invitation.Id);
            if (origin != null)
            {
                this.dbContext.Entry(origin).CurrentValues.SetValues(invitation);
            }
        }
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }

    public async Task<IdOperationResult> DeleteAsync(JoinOrganizationInvitation invitation)
    {
        this.dbContext.JoinOrganizationInvitations.Remove(invitation);
        await this.dbContext.SaveChangesAsync();
        return IdOperationResult.Success;
    }
}
