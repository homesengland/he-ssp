using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Config;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.Utilities;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.Programme.Contract;
using AffordableHomesAmountType = HE.Investments.FrontDoor.Shared.Project.Contract.AffordableHomesAmount;

namespace HE.Investments.FrontDoor.Domain.Services.Strategies;

public class AhpProjectConversionStrategy : IProjectConversionStrategy
{
    private readonly IProgrammeAvailabilityService _programmeAvailability;

    private readonly IProgrammeSettings _programmeSettings;

    public AhpProjectConversionStrategy(
        IProgrammeAvailabilityService programmeAvailability,
        IProgrammeSettings programmeSettings)
    {
        _programmeAvailability = programmeAvailability;
        _programmeSettings = programmeSettings;
    }

    public async Task<ApplicationType> Apply(ProjectEntity project, ProjectSitesEntity projectSites, CancellationToken cancellationToken)
    {
        return await IsProjectValidForAhpProject(project, cancellationToken) && AreSitesValidForAhpProject(projectSites) ? ApplicationType.Ahp : ApplicationType.Undefined;
    }

    private async Task<bool> IsProjectValidForAhpProject(ProjectEntity project, CancellationToken cancellationToken)
    {
        var isProjectValid = project.SupportActivities.Values.Count == 1
               && project.SupportActivities.Values.Contains(SupportActivityType.DevelopingHomes)
               && project.AffordableHomesAmount.AffordableHomesAmount is AffordableHomesAmountType.OnlyAffordableHomes
                   or AffordableHomesAmountType.OpenMarkedAndAffordableHomes
               && project.OrganisationHomesBuilt?.Value >= 0
               && project.IsSiteIdentified?.Value is true
               && project.IsSupportRequired?.Value is true or false
               && project.IsFundingRequired?.Value is true
               && project.RequiredFunding.Value is RequiredFundingOption.LessThan250K
                   or RequiredFundingOption.Between250KAnd1Mln
                   or RequiredFundingOption.Between1MlnAnd5Mln
                   or RequiredFundingOption.Between5MlnAnd10Mln
                   or RequiredFundingOption.Between10MlnAnd30Mln
                   or RequiredFundingOption.Between30MlnAnd50Mln
                   or RequiredFundingOption.MoreThan50Mln
               && project.IsProfit.Value is true or false;

        return isProjectValid &&
               await _programmeAvailability.IsStartDateValidForProgramme(
                   ProgrammeId.From(_programmeSettings.AhpProgrammeId),
                   project.ExpectedStartDate.Value,
                   cancellationToken);
    }

    private bool AreSitesValidForAhpProject(ProjectSitesEntity projectSites)
    {
        return projectSites.Sites.Count >= 1 && projectSites.Sites.All(IsSiteValidForAhpProject);
    }

    private bool IsSiteValidForAhpProject(ProjectSiteEntity site)
    {
        return PlanningStatusDivision.IsStatusAllowedForAhpProject(site.PlanningStatus.Value)
               && !LocalAuthorityDivision.IsLocalAuthorityNotAllowedForAhpProject(site.LocalAuthority?.Code.ToString())
               && site.HomesNumber?.Value is >= 1;
    }
}
