﻿using IDSubjects;
using IDSubjects.DependencyInjection;
using IDSubjects.Payments;
using IDSubjects.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for IDSubjects service injection.
/// </summary>
public static class IdSubjectsServiceCollectionExtensions
{

    /// <summary>
    /// 向基础设施添加AlphaID自然人标识管理功能。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IdentityBuilder AddIdSubjects(this IServiceCollection services, Action<IdSubjectsOptions>? setupAction = null)
    {
        // 由IDSubjects使用的服务。
        services.TryAddScoped<OrganizationManager>();
        services.TryAddScoped<OrganizationMemberManager>();
        services.TryAddScoped<OrganizationSearcher>();
        services.TryAddScoped<NaturalPersonIdentityErrorDescriber>();
        services.TryAddScoped<PersonBankAccountManager>();

        //添加基础标识
        var builder = services.AddIdentityCore<NaturalPerson>()
            .AddUserManager<NaturalPersonManager>()
            .AddUserValidator<PhoneNumberValidator>()
            ;

        if(setupAction != null)
        {
            services.Configure(setupAction);
        }

        return builder;
    }


}
