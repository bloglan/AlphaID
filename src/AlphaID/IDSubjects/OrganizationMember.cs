﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IdSubjects;

/// <summary>
///     组织的人员。
/// </summary>
[Table("OrganizationMember")]
[PrimaryKey(nameof(PersonId), nameof(OrganizationId))]
public class OrganizationMember
{
    /// <summary>
    /// </summary>
    protected OrganizationMember()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="organization"></param>
    /// <param name="person"></param>
    public OrganizationMember(GenericOrganization organization, NaturalPerson person)
    {
        OrganizationId = organization.Id;
        Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        PersonId = person.Id;
        Person = person ?? throw new ArgumentNullException(nameof(person));
    }

    /// <summary>
    ///     GenericOrganization Id.
    /// </summary>
    [MaxLength(50)]
    [Unicode(false)]
    public string OrganizationId { get; protected set; } = default!;

    /// <summary>
    ///     Person Id.
    /// </summary>
    [MaxLength(50)]
    [Unicode(false)]
    public string PersonId { get; protected set; } = default!;

    /// <summary>
    ///     GenericOrganization.
    /// </summary>
    [ForeignKey(nameof(OrganizationId))]
    public GenericOrganization Organization { get; protected set; } = default!;

    /// <summary>
    ///     Person.
    /// </summary>
    [ForeignKey(nameof(PersonId))]
    public NaturalPerson Person { get; protected set; } = default!;

    /// <summary>
    ///     部门。
    /// </summary>
    [MaxLength(50)]
    public string? Department { get; set; } = default!;

    /// <summary>
    ///     职务。
    /// </summary>
    [MaxLength(50)]
    public string? Title { get; set; } = default!;

    /// <summary>
    ///     备注。
    /// </summary>
    [MaxLength(50)]
    public string? Remark { get; set; } = default!;

    /// <summary>
    ///     Is Owner of the organization.
    /// </summary>
    public bool IsOwner { get; set; }

    /// <summary>
    ///     Membership visibility.
    /// </summary>
    public virtual MembershipVisibility Visibility { get; set; } = MembershipVisibility.Private;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Organization.Name}|{Person.UserName}|{(IsOwner ? "Owner" : "")}|{Visibility}";
    }
}