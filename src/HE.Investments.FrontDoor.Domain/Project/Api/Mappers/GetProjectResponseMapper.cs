using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.FrontDoor.Domain.Project.Api.Contract.Responses;

namespace HE.Investments.FrontDoor.Domain.Project.Api.Mappers;

internal static class GetProjectResponseMapper
{
    public static FrontDoorProjectDto Map(GetProjectResponse response)
    {
        // TODO: AB#98936 those 3 properties are not mapped: ProjectType, ProposedInterventions, LeadDirectorate. What should we do with them?
        return new FrontDoorProjectDto
        {
            ProjectId = response.ProjectRecordId.ToString(),
            ProjectName = response.ProjectName,
            CreatedOn = response.CreatedOn?.UtcDateTime,
            OrganisationId = response.OrganisationId,
            externalId = "auth0|65800f41617b364da56913f9", // TODO: AB#98936 map from response.PortalOwnerId when API will return external Id
            ProjectSupportsHousingDeliveryinEngland = response.ProjectSupportsHousingDeliveryInEngland,
            ActivitiesinThisProject = response.ActivitiesInThisProject?.ToList() ?? [],
            InfrastructureDelivered = response.InfrastructureDelivered?.ToList() ?? [],
            AmountofAffordableHomes = response.AmountOfAffordableHomes,
            PreviousResidentialBuildingExperience = response.PreviousResidentialBuildingExperience,
            IdentifiedSite = response.IdentifiedSite,
            GeographicFocus = response.GeographicFocus,
            Region = response.Region?.ToList() ?? [],
            LocalAuthority = response.LocalAuthority ?? string.Empty,
            LocalAuthorityName = response.LocalAuthorityName ?? string.Empty,
            LocalAuthorityCode = response.LocalAuthorityCode ?? string.Empty,
            NumberofHomesEnabledBuilt = response.NumberOfHomesEnabledBuilt,
            WouldyourprojectfailwithoutHEsupport = response.WouldYourProjectFailWithoutHeSupport,
            FundingRequired = response.FundingRequired,
            AmountofFundingRequired = response.AmountOfFundingRequired,
            IntentiontoMakeaProfit = response.IntentionToMakeAProfit,
            StartofProjectMonth = response.StartOfProjectMonth,
            StartofProjectYear = response.StartOfProjectYear,
            FrontDoorProjectContact = FrontDoorProjectContactMapper.Map(response.FrontDoorProjectContact),
        };
    }
}
