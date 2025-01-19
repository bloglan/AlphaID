using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlphaIdPlatform.Subjects;

/// <summary>
/// </summary>
[Owned]
[Table("OrganizationIdentifier")]
[PrimaryKey(nameof(Value), nameof(Type))]
public class OrganizationIdentifier
{
    /// <summary>
    /// </summary>
    [MaxLength(50)]
    [Unicode(false)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// </summary>
    [Column(TypeName = "varchar(30)")]
    public OrganizationIdentifierType Type { get; set; }

    /// <summary>
    /// </summary>
    [MaxLength(30)]
    public string Value { get; set; } = null!;
}