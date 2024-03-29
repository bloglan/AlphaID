﻿using IdSubjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlphaId.EntityFramework;

internal class NaturalPersonStore(IdSubjectsDbContext context) : NaturalPersonStoreBase
{
    public override IQueryable<NaturalPerson> Users => context.People.AsNoTracking();

    public override Task AddClaimsAsync(NaturalPerson user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        foreach (var claim in claims)
        {
            context.PersonClaims.Add(new NaturalPersonClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            });
        }
        return Task.FromResult(false);
    }

    public override Task AddLoginAsync(NaturalPerson user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        context.PersonLogins.Add(new NaturalPersonLogin()
        {
            LoginProvider = login.LoginProvider,
            ProviderDisplayName = login.ProviderDisplayName,
            ProviderKey = login.ProviderKey,
            UserId = user.Id,
        });
        return Task.FromResult(false);
    }

    public override async Task<IdentityResult> CreateAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        context.People.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> DeleteAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        context.People.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public override Task<NaturalPerson?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return context.People.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public override Task<NaturalPerson?> GetOriginalAsync(NaturalPerson person, CancellationToken cancellationToken)
    {
        return context.People.AsNoTrackingWithIdentityResolution().SingleOrDefaultAsync(p => p.Id == person.Id, cancellationToken: cancellationToken);
    }

    public override async Task<NaturalPerson?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.People.FindAsync([userId], cancellationToken: cancellationToken);
    }

    public override Task<NaturalPerson?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return context.People.SingleOrDefaultAsync(p => p.PhoneNumber == phoneNumber, cancellationToken);
    }

    public override async Task<NaturalPerson?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        var login = await context.PersonLogins.Include(naturalPersonLogin => naturalPersonLogin.User).FirstOrDefaultAsync(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);
        return login?.User;
    }

    public override Task<NaturalPerson?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return context.People.SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken: cancellationToken);
    }

    public override async Task<IList<Claim>> GetClaimsAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        return await context.PersonClaims.Where(c => c.UserId == user.Id).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken: cancellationToken);
    }

    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        return await context.PersonLogins
            .Where(l => l.UserId == user.Id)
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public override async Task<string?> GetTokenAsync(NaturalPerson user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        var token = await context.PersonTokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == loginProvider && t.Name == name, cancellationToken: cancellationToken);
        return token?.Value;
    }

    public override async Task<IList<NaturalPerson>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        var query = from userClaim in context.PersonClaims
                    join user in context.People on userClaim.UserId equals user.Id
                    where userClaim.ClaimValue == claim.Value && userClaim.ClaimType == claim.Type
                    select user;
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public override async Task RemoveClaimsAsync(NaturalPerson user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        foreach (var claim in claims)
        {
            var matchedClaims = await context.PersonClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in matchedClaims)
            {
                context.PersonClaims.Remove(c);
            }
        }
    }

    public override async Task RemoveLoginAsync(NaturalPerson user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        var login = await context.PersonLogins.FirstOrDefaultAsync(l => l.UserId == user.Id && l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken: cancellationToken);
        if (login != null)
        {
            context.PersonLogins.Remove(login);
        }
    }

    public override async Task RemoveTokenAsync(NaturalPerson user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        var token = await context.PersonTokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == loginProvider && t.Name == name, cancellationToken: cancellationToken);
        if (token != null)
            context.PersonTokens.Remove(token);
    }

    public override async Task ReplaceClaimAsync(NaturalPerson user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        var matchedClaims = await context.PersonClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.ClaimValue = newClaim.Value;
            matchedClaim.ClaimType = newClaim.Type;
        }
    }

    public override async Task SetTokenAsync(NaturalPerson user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
    {
        var token = await context.PersonTokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == loginProvider && t.Name == name, cancellationToken: cancellationToken);
        if (token == null)
        {
            context.PersonTokens.Add(new NaturalPersonToken()
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value,
                User = user,
            });
        }
        else
        {
            token.Value = value;
        }
    }

    public override async Task<IdentityResult> UpdateAsync(NaturalPerson user, CancellationToken cancellationToken)
    {
        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        //this.context.People.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

}
