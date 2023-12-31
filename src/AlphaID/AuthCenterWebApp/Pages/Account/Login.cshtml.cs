using AlphaIdPlatform.Identity;
using AuthCenterWebApp.Services;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdSubjects;
using IdSubjects.SecurityAuditing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthCenterWebApp.Pages.Account;

[SecurityHeaders]
[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly NaturalPersonManager userManager;
    private readonly SignInManager<NaturalPerson> signInManager;
    private readonly ILogger<LoginModel>? logger;
    private readonly IIdentityServerInteractionService interaction;
    private readonly Duende.IdentityServer.Services.IEventService events;
    private readonly IAuthenticationSchemeProvider schemeProvider;
    private readonly IIdentityProviderStore identityProviderStore;

    public ViewModel View { get; set; } = default!;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public LoginModel(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        Duende.IdentityServer.Services.IEventService events,
        NaturalPersonManager userManager,
        SignInManager<NaturalPerson> signInManager,
        ILogger<LoginModel>? logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
        this.interaction = interaction;
        this.schemeProvider = schemeProvider;
        this.identityProviderStore = identityProviderStore;
        this.events = events;
    }

    public async Task<IActionResult> OnGet(string? returnUrl)
    {
        await this.BuildModelAsync(returnUrl);
        if (this.View.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            this.logger?.LogInformation("we only have one option for logging in and it's an external provider");
            return this.RedirectToPage("/ExternalLogin/Challenge", new { scheme = this.View.ExternalLoginScheme, schemeDisplayName = this.View.ExternalLoginDisplayName, returnUrl });
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPost()
    {
        //检查我们是否在授权请求的上下文中
        var context = await this.interaction.GetAuthorizationContextAsync(this.Input.ReturnUrl);

        //用户点击了“取消”按钮
        if (this.Input.Button != "login")
        {
            if (context != null)
            {
                // 将用户的取消发送回 IdentityServer，以便它能拒绝 consent（即便客户端不需要consent）。
                // 这将会把访问被拒绝的响应发送回客户端。
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await this.interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(this.Input.ReturnUrl!);
                }

                return this.Redirect(this.Input.ReturnUrl!);
            }

            //由于我们没有有效的上下文，那么我们只需返回主页
            return this.Redirect("~/");
        }

        if (this.ModelState.IsValid)
        {
            //登录过程。
            var user = await this.userManager.FindByEmailAsync(this.Input.Username)
                ?? await this.userManager.FindByMobileAsync(this.Input.Username, this.HttpContext.RequestAborted)
                ?? await this.userManager.FindByNameAsync(this.Input.Username);
            if (user != null)
            {
                var result = await this.signInManager.PasswordSignInAsync(user, this.Input.Password, this.Input.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    await this.events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage(this.Input.ReturnUrl!);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return this.Redirect(this.Input.ReturnUrl!);
                    }

                    // request for a local page
                    if (this.Url.IsLocalUrl(this.Input.ReturnUrl))
                    {
                        return this.Redirect(this.Input.ReturnUrl);
                    }

                    if (string.IsNullOrEmpty(this.Input.ReturnUrl))
                    {
                        return this.Redirect("~/");
                    }

                    // user might have clicked on a malicious link - should be logged
                    throw new Exception("invalid return URL");
                }

                if (result.MustChangePassword())
                {

/* 项目“AuthCenterWebApp (net6.0)”的未合并的更改
在此之前:
                    var principal = this.GenerateMustChangePasswordPrincipal(user);
在此之后:
                    var principal = GenerateMustChangePasswordPrincipal(user);
*/
                    var principal = LoginModel.GenerateMustChangePasswordPrincipal(user);
                    await this.signInManager.SignOutAsync();
                    await this.HttpContext.SignInAsync(IdSubjectsIdentityDefaults.MustChangePasswordScheme, principal);
                    return this.RedirectToPage("ChangePassword", new { this.Input.ReturnUrl, RememberMe = this.Input.RememberLogin });
                }

                if (result.RequiresTwoFactor)
                {
                    return this.RedirectToPage("./LoginWith2fa", new { this.Input.ReturnUrl, RememberMe = this.Input.RememberLogin });
                }

                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
            }


            await this.events.RaiseAsync(new UserLoginFailureEvent(this.Input.Username, "invalid credentials", clientId: context?.Client.ClientId));
            this.ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
        }

        // something went wrong, show form with error
        await this.BuildModelAsync(this.Input.ReturnUrl);
        return this.Page();
    }

    private async Task BuildModelAsync(string? returnUrl)
    {
        this.Input = new InputModel
        {
            ReturnUrl = returnUrl
        };

        var context = await this.interaction.GetAuthorizationContextAsync(returnUrl);

        //在授权上下文中指定了IdP，因此跳过本地登录而直接转到指定的IdP。
        if (context?.IdP != null && await this.schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;
            var scheme = await this.schemeProvider.GetSchemeAsync(context.IdP);
            // this is meant to short circuit the UI and only trigger the one external IdP
            this.View = new ViewModel
            {
                EnableLocalLogin = local,
            };

            this.Input.Username = context.LoginHint ?? "";

            if (!local)
            {
                this.View.ExternalProviders = new ViewModel.ExternalProvider[]
                {
                    new()
                    {
                        AuthenticationScheme = context.IdP,
                        DisplayName = scheme!.DisplayName!,
                    }
                };
            }

            return;
        }

        var schemes = await this.schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ViewModel.ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var dynamicSchemes = (await this.identityProviderStore.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled)
            .Select(x => new ViewModel.ExternalProvider
            {
                AuthenticationScheme = x.Scheme,
                DisplayName = x.DisplayName ?? "",
            });
        providers.AddRange(dynamicSchemes);


        var allowLocal = true;
        var client = context?.Client;
        if (client != null)
        {
            allowLocal = client.EnableLocalLogin;
            if (client.IdentityProviderRestrictions.Any())
            {
                providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        this.View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }

    private static ClaimsPrincipal GenerateMustChangePasswordPrincipal(NaturalPerson person)
    {
        var identity = new ClaimsIdentity(IdSubjectsIdentityDefaults.MustChangePasswordScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, person.Id));
        return new ClaimsPrincipal(identity);
    }
    public class InputModel
    {
        [Required(ErrorMessage = "Validate_Required")]
        [Display(Name = "User name", Prompt = "Account name, email, mobile phone number, ID card number, etc.")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Validate_Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Display(Name = "Remember me on this device")]
        public bool RememberLogin { get; set; }

        public string? ReturnUrl { get; set; }

        public string Button { get; set; } = default!;
    }

    public class ViewModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<ExternalProvider> VisibleExternalProviders => this.ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => this.EnableLocalLogin == false && this.ExternalProviders.Count() == 1;
        public string? ExternalLoginScheme => this.IsExternalLoginOnly ? this.ExternalProviders.SingleOrDefault()?.AuthenticationScheme : null;

        public string? ExternalLoginDisplayName => this.IsExternalLoginOnly ? this.ExternalProviders.SingleOrDefault()?.DisplayName : null;

        public class ExternalProvider
        {
            public string DisplayName { get; set; } = default!;
            public string AuthenticationScheme { get; set; } = default!;
        }
    }

    public class LoginOptions
    {
        public static readonly bool AllowLocalLogin = true;
        public static readonly bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static readonly string InvalidCredentialsErrorMessage = "用户名或密码无效";
    }
}