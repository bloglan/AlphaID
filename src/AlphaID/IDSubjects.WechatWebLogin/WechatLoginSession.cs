﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdSubjects.WechatWebLogin;

/// <summary>
/// 
/// </summary>
[Table("WechatLoginSession")]
public class WechatLoginSession
{
    /// <summary>
    /// 
    /// </summary>
    protected WechatLoginSession() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="wechatAppId"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="resource"></param>
    /// <param name="redirectUri"></param>
    internal WechatLoginSession(string wechatAppId, string clientId, string clientSecret, string resource, string redirectUri)
    {
        this.WechatAppId = wechatAppId;
        this.ClientId = clientId;
        this.ClientSecret = clientSecret;
        this.Resource = resource;
        this.RedirectUri = redirectUri;
        this.Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        this.WhenExpires = DateTime.UtcNow.AddMinutes(10.0D);
    }

    /// <summary>
    /// 
    /// </summary>
    [Key]
    public string Id { get; protected set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(50), Unicode(false)]
    public string WechatAppId { get; protected set; } = default!;


    /// <summary>
    /// 
    /// </summary>
    [MaxLength(50), Unicode(false)]
    public string ClientId { get; protected set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(150), Unicode(false)]
    public string Resource { get; protected set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime WhenExpires { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(50), Unicode(false)]
    public string ClientSecret { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(150), Unicode(false)]
    public string RedirectUri { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(512), Unicode(false)]
    public string WechatOAuthToken { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(128), Unicode(false)]
    public string OpenId { get; set; } = default!;

    /// <summary>
    /// 微信AccessToken的过期秒数。
    /// </summary>
    public int WechatOAuthTokenExpiresIn { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime WechatOauthTokenExpires { get; set; } = default!;

    /// <summary>
    /// 移动电话。
    /// </summary>
    [MaxLength(18), Unicode(false)]
    public string Mobile { get; set; } = default!;

    /// <summary>
    /// 与此会话关联的微信用户。
    /// </summary>
    [NotMapped]
    public WechatUserIdentifier? WechatUser { get; internal set; } = default!;
}