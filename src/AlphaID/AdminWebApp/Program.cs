using AdminWebApp;
using AdminWebApp.Domain.Security;
using AdminWebApp.Infrastructure.DataStores;
using AdminWebApp.Services;
using AlphaId.RealName.EntityFramework;
using AlphaId.DirectoryLogon.EntityFramework;
using AlphaId.EntityFramework;
using AlphaId.EntityFramework.SecurityAuditing;
using AlphaId.PlatformServices.Aliyun;
using AlphaIdPlatform;
using AlphaIdPlatform.Debugging;
using AlphaIdPlatform.Platform;
using AlphaIdPlatform.RazorPages;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using IdentityModel;
using IdSubjects.ChineseName;
using IdSubjects.DirectoryLogon;
using IdSubjects.RealName;
using IdSubjects.SecurityAuditing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using Newtonsoft.Json;
// ReSharper disable StringLiteralTypo

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .WriteTo.EventLog(".NET Runtime", restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.Logger(lc =>
    {
        lc.ReadFrom.Configuration(context.Configuration);
        lc.Filter.ByIncludingOnly(log =>
            {
                if (log.Properties.TryGetValue("SourceContext", out var pv))
                {
                    var source = JsonConvert.DeserializeObject<string>(pv.ToString());
                    if (source == "Duende.IdentityServer.Events.DefaultEventService" || source == "IdSubjects.SecurityAuditing.DefaultEventService")
                    {
                        return true;
                    }
                }
                return false;
            })
            .WriteTo.MSSqlServer(
                builder.Configuration.GetConnectionString(nameof(IdSubjectsDbContext)),
                sinkOptions: new MSSqlServerSinkOptions() { TableName = "AuditLog" },
                columnOptions: new ColumnOptions()
                {
                    AdditionalColumns = new[]
                    {
                        new SqlColumn("EventId", SqlDbType.Int){PropertyName = "EventId.Id"},
                        new SqlColumn("Source", SqlDbType.NVarChar){PropertyName = "SourceContext"},
                    },
                }
            );
    });
});

//产品和系统URL信息。
builder.Services.Configure<ProductInfo>(builder.Configuration.GetSection("ProductInfo"));
builder.Services.Configure<SystemUrlInfo>(builder.Configuration.GetSection("SystemUrl"));

//程序资源
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

//区域和本地化
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("zh-CN"),
    };
    options.DefaultRequestCulture = new RequestCulture(culture: builder.Configuration["DefaultCulture"]!, uiCulture: builder.Configuration["DefaultCulture"]!);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


//配置RazorPages.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/", "RequireAdminRole");
    options.Conventions.AuthorizeFolder("/Account");
    options.Conventions.Add(new SubjectAnchorRouteModelConvention("/Detail", "People"));
    options.Conventions.Add(new SubjectAnchorRouteModelConvention("/Detail", "Organizations"));
    options.Conventions.Add(new SubjectAnchorRouteModelConvention("/Clients/Detail", "OpenIDConnect"));

})
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource));
    })
    .AddSessionStateTempDataProvider();

//启用API Controller
builder.Services.AddControllers();

//配置授权策略。
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
    {
        policy.RequireRole(RoleConstants.AdministratorsRole.Name);
    });
});

//启用服务器Session
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

//配置身份验证。
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.MetadataAddress = builder.Configuration["OidcClient:MetadataAddress"];
        options.ClientId = builder.Configuration["OidcClient:ClientId"];
        options.ClientSecret = builder.Configuration["OidcClient:ClientSecret"];
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true; //从UserInfoEndPoint取得更多用户信息。

        //hack 将name声明添加到完全类型“http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name”上。
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, JwtClaimTypes.Name);
        //options.ClaimActions.MapJsonKey("profile", JwtClaimTypes.Profile);
        options.ClaimActions.MapJsonKey("picture", JwtClaimTypes.Picture);
        options.ClaimActions.MapJsonKey("locale", JwtClaimTypes.Locale);
        options.ClaimActions.MapJsonKey("zoneinfo", JwtClaimTypes.ZoneInfo);

        options.Events = new OpenIdConnectEvents()
        {
            OnRedirectToIdentityProvider = context =>
            {
                if (!builder.Environment.IsDevelopment())
                {
                    var from = context.Request.Query["from"];
                    if (from.Contains("netauth.changingsoft.com"))
                    {
                        context.ProtocolMessage.SetParameter("acr_values", "idp:netauth.changingsoft.com");
                    }
                    else
                    {
                        //指示认证中心直接使用AD FS来处理用户登录。
                        //context.ProtocolMessage.SetParameter("acr_values", "idp:federal.changingsoft.com");
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDbContext<ConfigurationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OidcConfigurationDataConnection"));
}).AddScoped<ConfigurationStoreOptions>();
builder.Services.AddDbContext<PersistedGrantDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OidcPersistedGrantDataConnection"));
}).AddScoped<OperationalStoreOptions>();

builder.Services.AddDbContext<OperationalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(OperationalDbContext)));
});

//自然人管理器
var idSubjectsBuilder = builder.Services.AddIdSubjects();
idSubjectsBuilder
    .AddDefaultStores()
    .AddDbContext(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(IdSubjectsDbContext)), sqlOptions =>
        {
            sqlOptions.UseNetTopologySuite();
        });
    });

if (bool.Parse(builder.Configuration[FeatureSwitch.RealNameFeature] ?? "false"))
{
    idSubjectsBuilder.AddRealName()
        .AddDefaultStores()
        .AddDbContext(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(RealNameDbContext))));
}

if (bool.Parse(builder.Configuration[FeatureSwitch.DirectoryAccountManagementFeature] ?? "false"))
{
    idSubjectsBuilder.AddDirectoryLogin()
        .AddDefaultStores()
        .AddDbContext(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(DirectoryLogonDbContext))));
}

//身份证OCR识别
builder.Services.AddScoped<IChineseIdCardOcrService, AliyunChineseIdCardOcrService>();


builder.Services.AddScoped<ChinesePersonNamePinyinConverter>();
builder.Services.AddScoped<ChinesePersonNameFactory>();

builder.Services.AddScoped<LogonAccountManager>()
    .AddScoped<ILogonAccountStore, LogonAccountStore>()
    .AddScoped<IQueryableLogonAccountStore, QueryableLogonAccountStore>()
    .AddScoped<IDirectoryServiceStore, DirectoryServiceStore>();


//添加邮件发送器。
builder.Services.AddScoped<IEmailSender, SmtpMailSender>()
    .Configure<SmtpMailSenderOptions>(builder.Configuration.GetSection("SmtpMailSenderOptions"));

builder.Services.AddScoped<IdApiService>();


//令牌转换服务
builder.Services.AddScoped<IClaimsTransformation, ClaimTransformation>();

//目录服务
builder.Services.AddScoped<DirectoryServiceManager>()
    .AddScoped<IDirectoryServiceStore, DirectoryServiceStore>();
builder.Services.AddScoped<LogonAccountManager>()
    .AddScoped<ILogonAccountStore, LogonAccountStore>();

//安全角色管理
builder.Services.AddScoped<UserInRoleManager>()
    .AddScoped<IUserInRoleStore, UserInRoleStore>();

//当Debug模式时，覆盖注册先前配置以解除外部依赖
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailSender, NopEmailSender>();
    builder.Services.AddScoped<IShortMessageService, NopShortMessageService>();
    builder.Services.AddScoped<IVerificationCodeService, NopVerificationCodeService>();
    builder.Services.AddScoped<IChineseIdCardOcrService, DebugChineseIdCardOcrService>();
}

//安全审计日志
builder.Services.AddAuditLog()
    .AddDefaultStore()
    .AddDbContext(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(IdSubjectsDbContext)));
    });

var app = builder.Build();

//Pipelines.
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapRazorPages();
app.MapControllers();

app.Run();

namespace AdminWebApp
{
    public class Program { }
}