using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IdentityModel;
using IDSubjects;
using IDSubjects.ChineseName;
using IDSubjects.Subjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AuthCenterWebApp.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly NaturalPersonManager userManager;
    private readonly SignInManager<NaturalPerson> signInManager;
    private readonly IIdentityServerInteractionService interaction;
    private readonly ILogger<Callback> logger;
    private readonly IEventService events;
    private readonly ChinesePersonNamePinyinConverter chinesePersonNamePinyinConverter;
    private readonly ChinesePersonNameFactory chinesePersonNameFactory;

    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        NaturalPersonManager userManager,
        SignInManager<NaturalPerson> signInManager,
        ChinesePersonNamePinyinConverter chinesePersonNamePinyinConverter,
        ChinesePersonNameFactory chinesePersonNameFactory)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.interaction = interaction;
        this.logger = logger;
        this.events = events;
        this.chinesePersonNamePinyinConverter = chinesePersonNamePinyinConverter;
        this.chinesePersonNameFactory = chinesePersonNameFactory;
    }

    public async Task<IActionResult> OnGet()
    {
        // read external identity from the temporary cookie
        var result = await this.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (result.Succeeded != true)
        {
            throw new Exception("外部登录错误。");
        }

        var externalUser = result.Principal;

        if (this.logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
            this.logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        // lookup our user and external provider info
        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown userid");

        var provider = result.Properties.Items[".AuthScheme"]!;
        var providerDisplayName = result.Properties.Items["schemeDisplayName"]!;
        var providerUserId = userIdClaim.Value;
        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // find external user
        var user = await this.userManager.FindByLoginAsync(provider, providerUserId);
        //这可能是您可以启动用户注册的自定义工作流的位置
        if (user == null)
        {
            return this.RedirectToPage("/Account/BindLogin", new { returnUrl });
        }

        user ??= await this.AutoProvisionUserAsync(provider, providerDisplayName, providerUserId, externalUser.Claims);

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        this.CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        await this.signInManager.SignInWithClaimsAsync(user, localSignInProps, additionalLocalClaims);

        // delete temporary cookie used during external authentication
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // retrieve return URL

        // check if external login is in the context of an OIDC request
        var context = await this.interaction.GetAuthorizationContextAsync(returnUrl);
        await this.events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, user.UserName, true, context?.Client.ClientId));

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(returnUrl);
            }
        }

        return this.Redirect(returnUrl);
    }

    private async Task<NaturalPerson> AutoProvisionUserAsync(string provider, string providerDisplayName, string providerUserId, IEnumerable<Claim> claims)
    {

        // email
        var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        var upn = claims.FirstOrDefault(p => p.Type == ClaimTypes.Upn)?.Value ?? email; //如果没有upn，则使用email代替。

        var mobile = claims.FirstOrDefault(p => p.Type == "mobile")?.Value ?? claims.FirstOrDefault(p => p.Type == "http://schemas.changingsoft.com/ws/2013/05/identity/claims/mobile")?.Value;

        var firstName = claims.FirstOrDefault(p => p.Type == JwtClaimTypes.GivenName)?.Value ?? claims.FirstOrDefault(p => p.Type == ClaimTypes.GivenName)?.Value;

        var lastName = claims.FirstOrDefault(p => p.Type == JwtClaimTypes.FamilyName)?.Value ?? claims.FirstOrDefault(p => p.Type == ClaimTypes.Surname)?.Value;

        var displayName = claims.FirstOrDefault(p => p.Type == "displayName")?.Value ?? claims.FirstOrDefault(p => p.Type == "http://schemas.changingsoft.com/ws/2013/05/identity/claims/displayname")?.Value ?? lastName + firstName;

        var (phoneticSurname, phoneticGivenName) = this.chinesePersonNamePinyinConverter.Convert(lastName, firstName);
        var chinesePersonName = new ChinesePersonName(lastName, firstName, phoneticSurname, phoneticGivenName);
        var phoneticDisplayName = $"{chinesePersonName.PhoneticSurname} {chinesePersonName.PhoneticGivenName}".Trim();

        var phoneticSearchHint = phoneticDisplayName.Replace(" ", string.Empty);

        //Valid是否具备足够自动注册的条件，否则跳转手动注册。
        //
        var builder = new PersonBuilder(upn);



        if (email != null)
        {
            builder.SetEmail(email);
        }
        if (mobile != null)
        {
            builder.SetMobile(MobilePhoneNumber.Parse(mobile));
        }
        var user = builder.Person;
        user.SetName(chinesePersonName); //hack 需要改进。


        var filtered = new List<Claim>();
        // create a list of claims that we want to transfer into our store
        //

        var identityResult = await this.userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
            throw new Exception(identityResult.Errors.First().Description);

        if (filtered.Any())
        {
            identityResult = await this.userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded)
                throw new Exception(identityResult.Errors.First().Description);
        }

        identityResult = await this.userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, providerDisplayName));
        return !identityResult.Succeeded ? throw new Exception(identityResult.Errors.First().Description) : user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        // capture the idp used to login, so the session knows where the user came from
        localClaims.Add(new Claim(JwtClaimTypes.IdentityProvider, externalResult.Properties!.Items[".AuthScheme"]!));

        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal!.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}