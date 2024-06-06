using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using HE.Investments.TestsUtils.Extensions;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.IntegrationTests.Utils;

public class FrontDoorDataManipulator
{
    public static readonly LocalAuthority Oxford = new(new LocalAuthorityCode("7000178"), "Oxford");

    private readonly IProjectCrmContext _projectCrmContext;

    private readonly ISiteCrmContext _siteCrmContext;

    public FrontDoorDataManipulator(IProjectCrmContext projectCrmContext, ISiteCrmContext siteCrmContext)
    {
        _projectCrmContext = projectCrmContext;
        _siteCrmContext = siteCrmContext;
    }

    public async Task<(string ProjectId, string SiteId)> FrontDoorProjectEligibleForAhpExist(ILoginData loginData, string? projectName = null, string? siteName = null)
    {
        var projectDto = new FrontDoorProjectDto
        {
            ProjectName = projectName ?? "IT Project".WithTimestampSuffix(),
            ProjectSupportsHousingDeliveryinEngland = true,
            OrganisationId = ShortGuid.ToGuid(loginData.OrganisationId),
            externalId = loginData.UserGlobalId,
            ActivitiesinThisProject = new SupportActivitiesMapper().Map(new SupportActivities([SupportActivityType.DevelopingHomes])),
            AmountofAffordableHomes = new AffordableHomesAmountMapper().ToDto(AffordableHomesAmount.OnlyAffordableHomes),
            InfrastructureDelivered = new ProjectInfrastructureMapper().Map(new ProjectInfrastructure([InfrastructureType.IDoNotKnow])),
            PreviousResidentialBuildingExperience = 1,
            IdentifiedSite = true,
            WouldyourprojectfailwithoutHEsupport = true,
            FundingRequired = true,
            AmountofFundingRequired = new RequiredFundingMapper().Map(new RequiredFunding(RequiredFundingOption.Between30MlnAnd50Mln)),
            IntentiontoMakeaProfit = true,
            StartofProjectMonth = 5,
            StartofProjectYear = 2024,
        };

        var projectId = await _projectCrmContext.Save(projectDto, loginData.UserGlobalId, loginData.OrganisationId, CancellationToken.None);
        var siteId = await CreateFrontDoorSite(loginData, siteName, projectDto, projectId);

        return (projectId, siteId);
    }

    private async Task<string> CreateFrontDoorSite(ILoginData loginData, string? siteName, FrontDoorProjectDto projectDto, string projectId)
    {
        var siteDto = new FrontDoorProjectSiteDto
        {
            SiteName = siteName ?? $"Site for {projectDto.ProjectName}",
            NumberofHomesEnabledBuilt = 10,
            PlanningStatus = new PlanningStatusMapper().ToDto(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice),
            LocalAuthorityCode = Oxford.Code.Value,
            LocalAuthorityName = Oxford.Name,
        };

        var siteId = await _siteCrmContext.Save(projectId, siteDto, loginData.UserGlobalId, loginData.OrganisationId, CancellationToken.None);
        return siteId;
    }
}
