﻿namespace IdSubjects;

/// <summary>
///     组织搜索器。
/// </summary>
/// <remarks>
///     初始化组织搜索器。
/// </remarks>
/// <param name="organizationStore"></param>
public class OrganizationSearcher(IOrganizationStore organizationStore)
{
    /// <summary>
    ///     搜索。
    /// </summary>
    /// <param name="keywords"></param>
    /// <returns></returns>
    public IEnumerable<GenericOrganization> Search(string keywords)
    {
        keywords = keywords.Trim();
        if (string.IsNullOrEmpty(keywords))
            return [];

        var result = new HashSet<GenericOrganization>();

        IQueryable<GenericOrganization> mainResult = organizationStore.Organizations.Where(p =>
            p.Name.Contains(keywords) || p.UsedNames.Any(n => n.Name.Contains(keywords)));

        result.UnionWith(mainResult);
        return result;
    }
}