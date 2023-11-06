﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IDSubjects.DependencyInjection;

/// <summary>
/// IDSubjects builder for DI.
/// </summary>
public class IdSubjectsBuilder
{
    /// <summary>
    /// Gets the service collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Create new builder using incoming service collection.
    /// </summary>
    /// <param name="services"></param>
    public IdSubjectsBuilder(IServiceCollection services)
    {
        this.Services = services;
    }

    /// <summary>
    /// Add natural person store implementation into the system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IdSubjectsBuilder AddPersonStore<T>() where T : IUserStore<NaturalPerson>
    {
        this.Services.TryAddScoped(typeof(IUserStore<NaturalPerson>), typeof(T));
        return this;
    }

    /// <summary>
    /// Add generic organization store implementation into the system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IdSubjectsBuilder AddOrganizationStore<T>() where T : IOrganizationStore
    {
        this.Services.TryAddScoped(typeof(IOrganizationStore), typeof(T));
        return this;
    }

    /// <summary>
    /// Add organization member store implementation into the system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IdSubjectsBuilder AddOrganizationMemberStore<T>() where T : IOrganizationMemberStore
    {
        this.Services.TryAddScoped(typeof(IOrganizationMemberStore), typeof(T));
        return this;
    }
}
