using HE.Investments.Common.CRM.Model;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

public static class ProjectCrmFields
{
    public static readonly IReadOnlyList<string> ProjectToRead = new List<string>
    {
        nameof(invln_FrontDoorProjectPOC.invln_Name),
        nameof(invln_FrontDoorProjectPOC.invln_FrontDoorProjectPOCId),
        nameof(invln_FrontDoorProjectPOC.invln_AmountofAffordableHomes),
        nameof(invln_FrontDoorProjectPOC.invln_IdentifiedSite),
        nameof(invln_FrontDoorProjectPOC.invln_ActivitiesinThisProject),
        nameof(invln_FrontDoorProjectPOC.invln_ProjectSupportsHousingDeliveryinEngland),
        nameof(invln_FrontDoorProjectPOC.invln_PreviousResidentialBuildingExperience),
        nameof(invln_FrontDoorProjectPOC.invln_GeographicFocus),
        nameof(invln_FrontDoorProjectPOC.invln_Region),
        nameof(invln_FrontDoorProjectPOC.invln_NumberofHomesEnabledBuilt),
        nameof(invln_FrontDoorProjectPOC.invln_WouldyourprojectfailwithoutHEsupport),
        nameof(invln_FrontDoorProjectPOC.invln_FundingRequired),
        nameof(invln_FrontDoorProjectPOC.invln_AmountofFundingRequired),
        nameof(invln_FrontDoorProjectPOC.invln_InfrastructureDelivered),
        nameof(invln_FrontDoorProjectPOC.invln_IntentiontoMakeaProfit),
        nameof(invln_FrontDoorProjectPOC.invln_StartofProjectYear),
        nameof(invln_FrontDoorProjectPOC.invln_StartofProjectMonth),
        nameof(invln_FrontDoorProjectPOC.invln_LocalAuthorityId),
    };

    public static string FormatFields(this IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
