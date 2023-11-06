﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDSubjects.WechatWebLogin;

/// <summary>
/// 
/// </summary>
[Table("WechatAppClient")]
public class WechatAppClient
{
    /// <summary>
    /// 
    /// </summary>
    protected WechatAppClient() { }

    internal WechatAppClient(string clientId, string secret)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new ArgumentException($"“{nameof(clientId)}”不能为 Null 或空白", nameof(clientId));
        }

        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException($"“{nameof(secret)}”不能为 Null 或空白", nameof(secret));
        }

        this.ClientId = clientId;
        this.Secret = secret;
    }

    /// <summary>
    /// ClientId.
    /// </summary>
    [Key]
    [MaxLength(50), Unicode(false)]
    public string ClientId { get; protected set; } = default!;

    /// <summary>
    /// Secret.
    /// </summary>
    [MaxLength(100), Unicode(false)]
    public string Secret { get; protected internal set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> RedirectUriList { get; protected set; } = default!;

}
