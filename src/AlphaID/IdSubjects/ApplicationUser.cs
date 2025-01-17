using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdSubjects;

/// <summary>
///     表示一个自然人个体。
/// </summary>
[Index(nameof(WhenCreated))]
[Index(nameof(WhenChanged))]
public class ApplicationUser : IdentityUser
{
    /// <summary>
    ///     for persistence.
    /// </summary>
    public ApplicationUser()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="personName"></param>
    public ApplicationUser(string userName, HumanNameInfo personName) : this()
    {
        // ReSharper disable VirtualMemberCallInConstructor
        UserName = userName;
        HumanName = personName;
        // ReSharper restore VirtualMemberCallInConstructor
    }


    /// <summary>
    ///     获取一个值，指示用户上一次设置密码的时间。如果该值为null，或超过设定的最大更改密码期限，则用户在登录时必须强制更改密码。
    /// </summary>
    public virtual DateTimeOffset? PasswordLastSet { get; protected internal set; }


    /// <summary>
    ///     When Created.
    /// </summary>
    public virtual DateTimeOffset WhenCreated { get; protected internal set; }

    /// <summary>
    ///     When Changed.
    /// </summary>
    public virtual DateTimeOffset WhenChanged { get; set; }

    /// <summary>
    ///     启用或禁用该自然人。如果禁用，自然人不会出现在一般搜索结果中。但可以通过Id查询。
    /// </summary>
    public virtual bool Enabled { get; set; } = true;

    /// <summary>
    ///     用户名称
    /// </summary>
    [PersonalData]
    public virtual HumanNameInfo? HumanName { get; set; }

    /// <summary>
    ///     昵称。
    /// </summary>
    [PersonalData]
    [MaxLength(20)]
    public virtual string? NickName { get; set; }

    /// <summary>
    ///     性别。
    /// </summary>
    [Column(TypeName = "varchar(6)")]
    [Comment("性别")]
    [PersonalData]
    public virtual Gender? Gender { get; set; }

    /// <summary>
    ///     出生日期
    /// </summary>
    [PersonalData]
    public virtual DateOnly? DateOfBirth { get; set; }


    /// <summary>
    ///     User head image data.
    /// </summary>
    public virtual BinaryDataInfo? ProfilePicture { get; set; }

    /// <summary>
    ///     区域和语言选项
    /// </summary>
    [MaxLength(10)]
    [Unicode(false)]
    [PersonalData]
    public virtual string? Locale { get; protected internal set; }

    /// <summary>
    ///     用户所选择的时区。存储为IANA Time zone database名称。
    /// </summary>
    [MaxLength(50)]
    [Unicode(false)]
    [PersonalData]
    public virtual string? TimeZone { get; protected internal set; }

    /// <summary>
    ///     地址。
    /// </summary>
    [PersonalData]
    public virtual AddressInfo? Address { get; set; }

    /// <summary>
    ///     个人主页。
    /// </summary>
    [MaxLength(256)]
    [PersonalData]
    public virtual string? WebSite { get; set; }

    /// <summary>
    ///     Override.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return UserName!;
    }
}