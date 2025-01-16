﻿using System.Transactions;

namespace IdSubjects.Invitations;

/// <summary>
///     加入组织邀请管理器。
/// </summary>
/// <remarks>
/// </remarks>
/// <param name="store"></param>
/// <param name="personManager"></param>
/// <param name="organizationManager"></param>
/// <param name="memberManager"></param>
public class JoinOrganizationInvitationManager(
    IJoinOrganizationInvitationStore store,
    ApplicationUserManager personManager,
    OrganizationManager organizationManager,
    OrganizationMemberManager memberManager)
{
    internal TimeProvider TimeProvider { get; set; } = TimeProvider.System;

    /// <summary>
    ///     获取我收到的邀请。
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<JoinOrganizationInvitation> GetPendingInvitations(ApplicationUser person)
    {
        return store.Invitations.Where(i =>
            i.InviteeId == person.Id && !i.Accepted.HasValue && i.WhenExpired > TimeProvider.GetLocalNow());
    }

    /// <summary>
    ///     获取组织发出的邀请
    /// </summary>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<JoinOrganizationInvitation> GetIssuedInvitations(string organizationId)
    {
        return store.Invitations.Where(i => i.OrganizationId == organizationId);
    }

    /// <summary>
    ///     邀请某人加入到组织。
    /// </summary>
    /// <param name="organization">要加入的组织。</param>
    /// <param name="invitee">邀请人的用户名</param>
    /// <param name="inviter"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> InviteMemberAsync(GenericOrganization organization,
        ApplicationUser invitee,
        string inviter)
    {
        var errors = new List<string>();
        OrganizationMember? existsMember = await memberManager.GetMemberAsync(invitee.Id, organization.Id);
        if (existsMember != null)
            errors.Add("Person is a member of this organization.");

        if (store.Invitations.Any(i =>
                i.InviteeId == invitee.Id && i.OrganizationId == organization.Id &&
                i.WhenExpired > TimeProvider.GetUtcNow()))
            errors.Add("You've been sent invitation to this person.");
        if (errors.Count != 0)
            return IdOperationResult.Failed([.. errors]);

        return await store.CreateAsync(new JoinOrganizationInvitation
        {
            Inviter = inviter,
            OrganizationId = organization.Id,
            InviteeId = invitee.Id,
            WhenCreated = DateTimeOffset.UtcNow,
            WhenExpired = DateTimeOffset.UtcNow.AddDays(1.0f), //todo 应可以在设置中更改。
            ExpectVisibility = MembershipVisibility.Public //todo 邀请时应可填写期望的可见性。
        });
    }

    /// <summary>
    ///     接受邀请并加入组织。
    /// </summary>
    /// <param name="invitation"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> AcceptAsync(JoinOrganizationInvitation invitation)
    {
        if (invitation.Accepted.HasValue)
            return IdOperationResult.Failed("Invitation has been processed.");

        OrganizationMember? existedMember =
            await memberManager.GetMemberAsync(invitation.InviteeId, invitation.OrganizationId);
        if (existedMember != null) return IdOperationResult.Failed("Person is already a member.");

        using var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        ApplicationUser? person = await personManager.FindByIdAsync(invitation.InviteeId);
        GenericOrganization? organization = await organizationManager.FindByIdAsync(invitation.OrganizationId);
        IdOperationResult? result = null;
        if (organization != null && person != null)
            result = await memberManager.CreateAsync(
                new OrganizationMember(organization, person)
                    { Visibility = invitation.ExpectVisibility });
        invitation.Accepted = true;
        await store.UpdateAsync(invitation);
        trans.Complete();

        return result ?? IdOperationResult.Success;
    }

    /// <summary>
    ///     拒绝邀请。
    /// </summary>
    /// <param name="invitation"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> RefuseAsync(JoinOrganizationInvitation invitation)
    {
        if (invitation.Accepted.HasValue)
            return IdOperationResult.Failed("Invitation has been processed.");

        invitation.Accepted = false;
        return await store.UpdateAsync(invitation);
    }

    /// <summary>
    /// Find invitation by id.
    /// </summary>
    /// <param name="invitationId"></param>
    /// <returns></returns>
    public ValueTask<JoinOrganizationInvitation?> FindById(int invitationId)
    {
        return ValueTask.FromResult(store.Invitations.FirstOrDefault(i => i.Id == invitationId));
    }

    /// <summary>
    /// Revoke invitation.
    /// </summary>
    /// <param name="invitation"></param>
    /// <returns></returns>
    public Task<IdOperationResult> Revoke(JoinOrganizationInvitation invitation)
    {
        return store.DeleteAsync(invitation);
    }
}