using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdSubjects;
using IdSubjects.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace AuthCenterWebApp.Services;

/// <summary>
///     自然人资源所有者验证器。
/// </summary>
/// <remarks>
///     该验证器会考虑<see cref="ApplicationUser.PasswordLastSet"></see>属性以确保密码有效。
/// </remarks>
/// <param name="userManager"></param>
/// <param name="signInManager"></param>
/// <param name="logger"></param>
public class PersonResourceOwnerPasswordValidator(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<PersonResourceOwnerPasswordValidator> logger,
    IOptions<IdSubjectsOptions> options,
    TimeProvider timeProvider)
#pragma warning disable CS9107 // 参数捕获到封闭类型状态，其值也传递给基构造函数。该值也可能由基类捕获。
    : ResourceOwnerPasswordValidator<ApplicationUser>(userManager, signInManager, logger)
#pragma warning restore CS9107 // 参数捕获到封闭类型状态，其值也传递给基构造函数。该值也可能由基类捕获。
{
    public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(context.UserName);
        if (user != null)
        {
            if (options.Value.Password.EnablePassExpires)
            {
                if (!user.PasswordLastSet.HasValue || user.PasswordLastSet.Value < timeProvider.GetUtcNow().AddDays(0 - options.Value.Password.PasswordExpiresDay))
                {
                    logger.LogInformation(
                        "Authentication failed for username: {username}, reason: User must change password before first login.",
                        context.UserName);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                    return;
                }
            }
        }
        await base.ValidateAsync(context);
    }
}