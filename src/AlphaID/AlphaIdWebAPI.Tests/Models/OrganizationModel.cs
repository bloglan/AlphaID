﻿namespace AlphaIdWebAPI.Tests.Models;
internal record OrganizationModel(string? Domicile, string? Contact, string? LegalPersonName, DateTime? Expires)
{
    public string SubjectId { get; set; } = default!;
    public string Name { get; set; } = default!;
}
