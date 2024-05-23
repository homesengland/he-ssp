extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;
using HE.Investments.FrontDoor.Shared.Project.Contract;
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

    public async Task<string> CreateProjectEligibleForAhp(UserAccount userAccount)
    {
        var projectDate = new ProjectData();
        var projectDto = new FrontDoorProjectDto
        {
            ProjectName = "IT Project".WithTimestampSuffix(),
            ProjectSupportsHousingDeliveryinEngland = true,
            OrganisationId = ShortGuid.ToGuid(userAccount.SelectedOrganisationId().Value),
            externalId = userAccount.UserGlobalId.Value,
            ActivitiesinThisProject = new SupportActivitiesMapper().Map(new SupportActivities([projectDate.ActivityType])),
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

        var projectId = await _projectCrmContext.Save(projectDto, userAccount, CancellationToken.None);
        var siteDto = new FrontDoorProjectSiteDto
        {
            SiteName = $"Site for {projectDto.ProjectName}",
            NumberofHomesEnabledBuilt = 10,
            PlanningStatus = new PlanningStatusMapper().ToDto(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice),
            LocalAuthorityCode = LocalAuthorities.Oxford.Code.Value,
            LocalAuthorityName = LocalAuthorities.Oxford.Name,
        };

        await _siteCrmContext.Save(projectId, siteDto, userAccount, CancellationToken.None);
        return projectId;
    }
}
