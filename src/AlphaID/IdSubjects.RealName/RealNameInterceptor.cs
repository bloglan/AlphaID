﻿using IdSubjects.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdSubjects.RealName;

internal class RealNameInterceptor : NaturalPersonInterceptor
{
    private readonly ILogger<RealNameInterceptor>? logger;
    private readonly IRealNameAuthenticationStore store;

    public RealNameInterceptor(ILogger<RealNameInterceptor>? logger, IRealNameAuthenticationStore store)
    {
        this.logger = logger;
        this.store = store;
    }

    private IEnumerable<RealNameAuthentication> pendingAuthentications = Enumerable.Empty<RealNameAuthentication>();

    public override async Task<IdentityResult> PreUpdateAsync(NaturalPersonManager personManager, NaturalPerson person)
    {
        List<IdentityError> errors = new();
        var personAuthentications = this.store.FindByPerson(person);
        if (!personAuthentications.Any())
        {
            this.logger?.LogDebug("没有找到{person}的实名记录，不执行操作。", person);
            return IdentityResult.Success;
        }

        this.pendingAuthentications = personAuthentications.Where(a => !a.Applied).ToList();
        if (this.pendingAuthentications.Any())
        {
            foreach (var authentication in this.pendingAuthentications)
            {
                authentication.ApplyToPerson(person);
                this.logger?.LogDebug("拦截器使用{authentication}覆盖了{person}的信息。", authentication, person);
            }
            return IdentityResult.Success;
        }

        //检查是否有对PersonName等作出的更改。
        var origin = await personManager.GetOriginalAsync(person);
        if (!origin.PersonName.Equals(person.PersonName))
        {
            this.logger?.LogDebug("自然人名称不相等。原始 {origin}，传入 {incoming}。", origin.PersonName, person.PersonName);
            errors.Add(new IdentityError() { Code = "CannotEditPersonName", Description = "Cannot change person name when realname passed." });
        }
        if (origin.DateOfBirth != person.DateOfBirth)
            errors.Add(new IdentityError() { Code = "CannotEditBirthDate", Description = "Cannot change birth date when realname passed." });
        if (origin.Gender != person.Gender)
            errors.Add(new IdentityError() { Code = "CannotEditGender", Description = "Cannot change gender when realname passed." });

        return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }

    public override async Task PostUpdateAsync(NaturalPersonManager personManager, NaturalPerson person)
    {
        foreach (var authentication in this.pendingAuthentications)
        {
            authentication.Applied = true;
            await this.store.UpdateAsync(authentication);
            this.logger?.LogDebug("已将{authentication}的应用状态改为已更改。", authentication);
        }

    }

    public override async Task PostDeleteAsync(NaturalPersonManager personManager, NaturalPerson person)
    {
        await this.store.DeleteByPersonIdAsync(person.Id);
    }
}