using System.Collections.Immutable;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public static class ProjectCrmFields
{
    public static readonly IReadOnlyList<string> ProjectToUpdate = new List<string>
    {
        nameof(invln_FrontDoorProjectPOC.invln_Name), nameof(invln_FrontDoorProjectPOC.invln_FrontDoorProjectPOCId),
    };

    public static readonly IReadOnlyList<string> ProjectToRead = ProjectToUpdate.ToList()
        .Append(new List<string>
        {
            nameof(invln_FrontDoorProjectPOC.invln_identifiedsiteName),
            nameof(invln_FrontDoorProjectPOC.invln_ActivitiesinThisProject),
            nameof(invln_FrontDoorProjectPOC.invln_activitiesinthisprojectName),
            nameof(invln_FrontDoorProjectPOC.invln_ProjectSupportsHousingDeliveryinEngland),
            nameof(invln_FrontDoorProjectPOC.invln_AmountofAffordableHomes),
            nameof(invln_FrontDoorProjectPOC.invln_PreviousResidentialBuildingExperience),
            nameof(invln_FrontDoorProjectPOC.invln_IdentifiedSite),
        })
        .ToImmutableList();

    public static string FormatFields(this IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
