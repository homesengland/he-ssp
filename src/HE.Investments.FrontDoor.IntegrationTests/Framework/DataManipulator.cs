extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

public class DataManipulator
{
    private readonly IProjectCrmContext _projectCrmContext;

    private readonly ISiteCrmContext _siteCrmContext;

    public DataManipulator(IProjectCrmContext projectCrmContext, ISiteCrmContext siteCrmContext)
    {
        _projectCrmContext = projectCrmContext;
        _siteCrmContext = siteCrmContext;
    }

    public async Task<string> FrontDoorProjectEligibleForAhpExist(ILoginData loginData)
    {
        var projectDto = new FrontDoorProjectDto
        {
            ProjectName = "IT Project".WithTimestampSuffix(),
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
        var siteDto = new FrontDoorProjectSiteDto
        {
            SiteName = $"Site for {projectDto.ProjectName}",
            NumberofHomesEnabledBuilt = 10,
            PlanningStatus = new PlanningStatusMapper().ToDto(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice),
            LocalAuthorityCode = LocalAuthorities.Oxford.Code.Value,
            LocalAuthorityName = LocalAuthorities.Oxford.Name,
        };

        await _siteCrmContext.Save(projectId, siteDto, loginData.UserGlobalId, loginData.OrganisationId, CancellationToken.None);
        return projectId;
    }
}
