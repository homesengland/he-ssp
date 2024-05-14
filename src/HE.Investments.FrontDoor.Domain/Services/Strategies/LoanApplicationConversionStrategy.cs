using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.Utilities;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using AffordableHomesAmountType = HE.Investments.FrontDoor.Shared.Project.Contract.AffordableHomesAmount;

namespace HE.Investments.FrontDoor.Domain.Services.Strategies;

public class LoanApplicationConversionStrategy : IProjectConversionStrategy
{
    public ApplicationType Apply(ProjectEntity project, ProjectSitesEntity projectSites)
    {
        return IsProjectValidForLoanApplication(project) && AreSitesValidForLoanApplication(projectSites) ? ApplicationType.Loans : ApplicationType.Undefined;
    }

    private bool IsProjectValidForLoanApplication(ProjectEntity project)
    {
        return project.SupportActivities.Values.Count == 1
               && project.SupportActivities.Values.Contains(SupportActivityType.DevelopingHomes)
               && project.AffordableHomesAmount.AffordableHomesAmount is AffordableHomesAmountType.OnlyOpenMarketHomes
                   or AffordableHomesAmountType.OpenMarkedAndRequiredAffordableHomes
               && project.OrganisationHomesBuilt?.Value <= 2000
               && project.IsSiteIdentified?.Value == true
               && project.IsSupportRequired?.Value == true
               && project.IsFundingRequired?.Value == true
               && project.RequiredFunding.Value is RequiredFundingOption.Between250KAnd1Mln
                   or RequiredFundingOption.Between1MlnAnd5Mln
                   or RequiredFundingOption.Between5MlnAnd10Mln
               && project.IsProfit.Value == true
               && DateTimeUtil.IsDateWithinXYearsFromNow(project.ExpectedStartDate.Value?.ToDateTime(TimeOnly.MinValue), 2);
    }

    private bool AreSitesValidForLoanApplication(ProjectSitesEntity projectSites)
    {
        return projectSites.Sites.Count == 1 && projectSites.Sites.All(IsSiteValidForLoanApplication);
    }

    private bool IsSiteValidForLoanApplication(ProjectSiteEntity site)
    {
        return PlanningStatusDivision.IsStatusAllowedForLoanApplication(site.PlanningStatus.Value)
               && !LocalAuthorityDivision.IsLocalAuthorityNotAllowedForLoanApplication(site.LocalAuthority?.Code.ToString())
               && site.HomesNumber?.Value is >= 5 and <= 200;
    }
}
