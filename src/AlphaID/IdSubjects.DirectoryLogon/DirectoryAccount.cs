﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace IdSubjects.DirectoryLogon;

/// <summary>
/// Logon Account
/// </summary>
[Table("LogonAccount")]
[PrimaryKey(nameof(PersonId), nameof(ServiceId))]
public class DirectoryAccount
{
    /// <summary>
    /// 
    /// </summary>
    protected DirectoryAccount() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceDescriptor"></param>
    /// <param name="personId"></param>
    public DirectoryAccount(DirectoryServiceDescriptor serviceDescriptor, string personId)
    {
        this.DirectoryServiceDescriptor = serviceDescriptor;
        this.ServiceId = serviceDescriptor.Id;
        this.PersonId = personId;
    }

    /// <summary>
    /// PersonId.
    /// </summary>
    [MaxLength(50), Unicode(false)]
    public string PersonId { get; protected internal set; } = default!;

    /// <summary>
    /// 目录对象的objectGUID。
    /// </summary>
    [MaxLength(50), Unicode(false)]
    public string ObjectId { get; protected internal set; } = default!;

    /// <summary>
    /// 目录服务Id.
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// 目录服务。
    /// </summary>
    [ForeignKey(nameof(ServiceId))]
    public virtual DirectoryServiceDescriptor DirectoryServiceDescriptor { get; protected set; } = default!;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    internal UserPrincipal? GetUserPrincipal()
    {
        var context = this.DirectoryServiceDescriptor.GetRootContext();
        return UserPrincipal.FindByIdentity(context, this.ObjectId);

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    internal void SetPassword(string? password, bool mustChangePassword = false)
    {
        using var user = this.GetUserPrincipal();
        ArgumentNullException.ThrowIfNull(user);

        user.SetPassword(password);

        var entry = (DirectoryEntry)user.GetUnderlyingObject();
        if (mustChangePassword)
            entry.Properties["pwdLastSet"][0] = 0;
    }
}
