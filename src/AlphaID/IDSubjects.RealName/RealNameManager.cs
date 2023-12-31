﻿namespace IdSubjects.RealName;

/// <summary>
/// 实名认证管理器。
/// </summary>
public class RealNameManager
{
    private readonly IRealNameAuthenticationStore store;
    private readonly NaturalPersonManager naturalPersonManager;

    /// <summary>
    /// 初始化实名认证管理器。
    /// </summary>
    /// <param name="store"></param>
    /// <param name="naturalPersonManager"></param>
    public RealNameManager(IRealNameAuthenticationStore store, NaturalPersonManager naturalPersonManager)
    {
        this.store = store;
        this.naturalPersonManager = naturalPersonManager;
    }

    /// <summary>
    /// 获取可查询的实名认证信息集合。
    /// </summary>
    public IQueryable<RealNameAuthentication> Authentications => this.store.Authentications;


    /// <summary>
    /// 获取与自然人相关的实名状态信息。
    /// </summary>
    /// <param name="person"></param>
    /// <returns>与自然人相关的实名状态。如果没有，则返回null。</returns>
    public virtual IEnumerable<RealNameAuthentication> GetAuthentications(NaturalPerson person)
    {
        return this.store.FindByPerson(person);
    }

    /// <summary>
    /// 向指定的自然人添加实名认证信息。
    /// </summary>
    /// <param name="person"></param>
    /// <param name="authentication"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> AuthenticateAsync(NaturalPerson person, RealNameAuthentication authentication)
    {
        authentication.PersonId = person.Id;
        var result = await this.store.CreateAsync(authentication);
        if (!result.Succeeded)
            return result;

        //为 person 应用更改。
        var identityResult = await this.naturalPersonManager.UpdateAsync(person);
        if (!identityResult.Succeeded)
            return IdOperationResult.Failed(identityResult.Errors.Select(e => e.Description).ToArray());

        return IdOperationResult.Success;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="authentication"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> RemoveAsync(RealNameAuthentication authentication)
    {
        return await this.store.DeleteAsync(authentication);
    }

    internal async Task<IdOperationResult> UpdateAsync(RealNameAuthentication authentication)
    {
        return await this.store.UpdateAsync(authentication);
    }

    internal bool HasAuthenticated(NaturalPerson person)
    {
        return this.store.Authentications.Any(a => a.PersonId == person.Id);
    }

    internal IEnumerable<RealNameAuthentication> GetPendingAuthentications(NaturalPerson person)
    {
        return this.store.FindByPerson(person).Where(a => !a.Applied);
    }

    internal async Task ClearAsync(NaturalPerson person)
    {
        await this.store.DeleteByPersonIdAsync(person.Id);
    }

    /// <summary>
    /// 查找指定的实名认证信息。
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RealNameAuthentication?> FindByIdAsync(string id)
    {
        return await this.store.FindByIdAsync(id);
    }
}
