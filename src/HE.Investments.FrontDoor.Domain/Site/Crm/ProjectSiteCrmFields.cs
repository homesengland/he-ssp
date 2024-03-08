using System.Collections.Immutable;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

public static class ProjectSiteCrmFields
{
    public static readonly IReadOnlyList<string> SiteToUpdate = new List<string>
    {
        nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectId),
        nameof(invln_FrontDoorProjectSitePOC.invln_Name),
        nameof(invln_FrontDoorProjectSitePOC.invln_PlanningStatus),
        nameof(invln_FrontDoorProjectSitePOC.invln_NumberofHomesEnabledBuilt),
    };

    public static readonly IReadOnlyList<string> SiteToRead = SiteToUpdate
        .ToImmutableList();

    public static string FormatFields(this IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
