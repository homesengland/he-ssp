using HE.Investments.Common.CRM.Model;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

public static class ProjectSiteCrmFields
{
    public static readonly IReadOnlyList<string> SiteToRead =
    [
        nameof(invln_FrontDoorProjectSitePOC.invln_FrontDoorProjectId),
        nameof(invln_FrontDoorProjectSitePOC.invln_Name),
        nameof(invln_FrontDoorProjectSitePOC.invln_PlanningStatus),
        nameof(invln_FrontDoorProjectSitePOC.invln_NumberofHomesEnabledBuilt),
        nameof(invln_FrontDoorProjectSitePOC.invln_LocalAuthorityId),
        nameof(invln_FrontDoorProjectSitePOC.CreatedOn),
    ];

    public static string FormatFields(this IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
