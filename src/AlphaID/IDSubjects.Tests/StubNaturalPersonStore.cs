﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace IdSubjects.Tests;

public class StubNaturalPersonStore : NaturalPersonStoreBase
{
    private readonly HashSet<NaturalPerson> _set = [];
    private readonly HashSet<NaturalPerson> _trackedSet = [];

    public override IQueryable<NaturalPerson> Users => _set.AsQueryable();

    public override Task AddClaimsAsync(NaturalPerson user,
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task AddLoginAsync(NaturalPerson user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<IdentityResult> CreateAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        _trackedSet.Add(user);
        _set.Add(Clone(user)!);
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> DeleteAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        _trackedSet.Remove(user);
        NaturalPerson origin = _set.Single(p => p.Id == user.Id);
        _set.Remove(origin);
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<NaturalPerson?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        NaturalPerson? result = _trackedSet.FirstOrDefault(p => p.NormalizedEmail == normalizedEmail);
        result ??= Clone(_set.FirstOrDefault(p => p.NormalizedEmail == normalizedEmail));

        if (result != null)
            _trackedSet.Add(result);
        return Task.FromResult(result);
    }

    public override Task<NaturalPerson?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        NaturalPerson? result = _trackedSet.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
        result ??= Clone(_set.FirstOrDefault(p => p.PhoneNumber == phoneNumber));

        if (result != null)
            _trackedSet.Add(result);
        return Task.FromResult(result);
    }

    public override Task<NaturalPerson?> GetOriginalAsync(NaturalPerson person, CancellationToken cancellationToken)
    {
        return Task.FromResult(_set.FirstOrDefault(p => p.Id == person.Id));
    }

    public override Task<NaturalPerson?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        NaturalPerson? result = _trackedSet.FirstOrDefault(p => p.Id == userId);
        if (result == null)
        {
            result = Clone(_set.FirstOrDefault(p => p.Id == userId));
            if (result != null)
                _trackedSet.Add(result);
        }

        return Task.FromResult(result);
    }

    public override Task<NaturalPerson?> FindByLoginAsync(string loginProvider,
        string providerKey,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<NaturalPerson?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        NaturalPerson? result = _trackedSet.FirstOrDefault(p => p.NormalizedUserName == normalizedUserName);
        result ??= Clone(_set.FirstOrDefault(p => p.NormalizedUserName == normalizedUserName));

        if (result != null)
            _trackedSet.Add(result);
        return Task.FromResult(result);
    }

    public override Task<IList<Claim>> GetClaimsAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<IList<UserLoginInfo>> GetLoginsAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<string?> GetTokenAsync(NaturalPerson user,
        string loginProvider,
        string name,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<IList<NaturalPerson>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task RemoveClaimsAsync(NaturalPerson user,
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task RemoveLoginAsync(NaturalPerson user,
        string loginProvider,
        string providerKey,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task RemoveTokenAsync(NaturalPerson user,
        string loginProvider,
        string name,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task ReplaceClaimAsync(NaturalPerson user,
        Claim claim,
        Claim newClaim,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task SetTokenAsync(NaturalPerson user,
        string loginProvider,
        string name,
        string? value,
        CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public override Task<IdentityResult> UpdateAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        _trackedSet.Add(user);

        NaturalPerson? found = _set.FirstOrDefault(p => p.Id == user.Id);
        if (found != null)
        {
            _set.Remove(found);
            _set.Add(Clone(user)!);
        }

        return Task.FromResult(IdentityResult.Success);
    }

    private static T? Clone<T>(T? source)
    {
        if (source == null)
            return source;
        return TransExp<T, T>.Trans(source);
    }
}