using System.Diagnostics;

namespace IdSubjects;

/// <summary>
/// GenericOrganization Member Manager.
/// </summary>
public class OrganizationMemberManager
{
    private readonly IOrganizationMemberStore store;

    /// <summary>
    /// Init GenericOrganization Member Manager via GenericOrganization Member store.
    /// </summary>
    /// <param name="store"></param>
    public OrganizationMemberManager(IOrganizationMemberStore store)
    {
        this.store = store;
    }

    /// <summary>
    /// Get the member.
    /// </summary>
    /// <param name="person"></param>
    /// <param name="organization"></param>
    /// <returns></returns>
    public Task<OrganizationMember?> GetMemberAsync(NaturalPerson person, GenericOrganization organization)
    {
        var result = this.store.OrganizationMembers.FirstOrDefault(p => p.PersonId == person.Id && p.OrganizationId == organization.Id);
        return Task.FromResult(result);
    }

    /// <summary>
    /// Get members of the organization.
    /// </summary>
    /// <param name="organization">A organization that members to get.</param>
    /// <param name="visitor">The person who access this system. null if anonymous access.</param>
    /// <returns></returns>
    public Task<IEnumerable<OrganizationMember>> GetVisibleMembersAsync(GenericOrganization organization, NaturalPerson? visitor)
    {
        var members = this.store.OrganizationMembers.Where(p => p.OrganizationId == organization.Id);
        Debug.Assert(members != null);
        var visibilityLevel = MembershipVisibility.Public;
        if (visitor != null)
        {
            visibilityLevel = MembershipVisibility.AuthenticatedUser;
            if (members.Any(m => m.PersonId == visitor.Id))
                visibilityLevel = MembershipVisibility.Private; //Visitor is a member of the organization.
        }
        return Task.FromResult(members.Where(m => m.Visibility >= visibilityLevel).AsEnumerable());
    }

    /// <summary>
    /// Get organization members.
    /// </summary>
    /// <param name="organization">Organization</param>
    /// <returns></returns>
    public Task<IEnumerable<OrganizationMember>> GetMembersAsync(GenericOrganization organization)
    {
        var members = this.store.OrganizationMembers.Where(p => p.OrganizationId == organization.Id);
        return Task.FromResult(members.AsEnumerable());
    }

    /// <summary>
    /// 以访问者visitor的视角检索指定用户的组织成员身份。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 如果
    /// </para>
    /// </remarks>
    /// <param name="person">要检索组织成员身份的目标用户。</param>
    /// <param name="visitor">访问者。如果传入null，代表匿名访问者。</param>
    /// <returns></returns>
    public IEnumerable<OrganizationMember> GetVisibleMembersOf(NaturalPerson person, NaturalPerson? visitor)
    {
        //获取目标person的所有组织身份。
        var members = this.store.OrganizationMembers.Where(p => p.PersonId == person.Id);
        Debug.Assert(members != null);

        if (visitor == null)
            return members.Where(m => m.Visibility == MembershipVisibility.Public);

        //获取访问者的所属组织Id列表。
        var visitorMemberOfOrgIds = this.store.OrganizationMembers.Where(m => m.PersonId == visitor.Id).Select(m => m.OrganizationId).ToList();

        return members.Where(m =>
            m.Visibility >= MembershipVisibility.AuthenticatedUser || (m.Visibility == MembershipVisibility.Private &&
                                                                       visitorMemberOfOrgIds
                                                                           .Contains(m.OrganizationId)));

        var visibilityLevel = MembershipVisibility.Public;
        if (visitor != null)
        {
            visibilityLevel = MembershipVisibility.AuthenticatedUser;
            if (members.Any(m => m.PersonId == visitor.Id))
                visibilityLevel = MembershipVisibility.Private; //Visitor is a member of the organization.
        }
        return members.Where(m => m.Visibility >= visibilityLevel).AsEnumerable();
    }

    /// <summary>
    /// 获取个人的组织成员身份。
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public Task<IEnumerable<OrganizationMember>> GetMembersOfAsync(NaturalPerson person)
    {
        var members = this.store.OrganizationMembers.Where(p => p.PersonId == person.Id);
        return Task.FromResult(members.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> CreateAsync(OrganizationMember member)
    {
        if (this.store.OrganizationMembers.Any(p => p.OrganizationId == member.OrganizationId && p.PersonId == member.PersonId))
            return IdOperationResult.Failed(Resources.MembershipExists);
        await this.store.CreateAsync(member);
        return IdOperationResult.Success;
    }

    /// <summary>
    /// Take person leave out the organization.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> LeaveOrganizationAsync(OrganizationMember member)
    {
        var members = this.store.OrganizationMembers.Where(m => m.OrganizationId == member.OrganizationId);
        
        if (member.IsOwner && members.Count(m => m.IsOwner) <= 1)
            return IdOperationResult.Failed(Resources.LastOwnerCannotLeave);

        return await this.store.DeleteAsync(member);
    }

    /// <summary>
    /// 移除用户成员身份，无论用户是否是组织所有者也是如此。
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public Task<IdOperationResult> RemoveAsync(OrganizationMember member)
    {
        return this.store.DeleteAsync(member);
    }

    /// <summary>
    /// Update organization member info.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public Task<IdOperationResult> UpdateAsync(OrganizationMember member)
    {
        return this.store.UpdateAsync(member);
    }
}
