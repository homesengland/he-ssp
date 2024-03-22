using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Services;

public class EligibilityService : IEligibilityService
{
    private readonly IProjectRepository _projectRepository;

    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public EligibilityService(
        IProjectRepository projectRepository,
        IAccountUserContext accountUserContext,
        ISiteRepository siteRepository)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<ApplicationType> GetEligibleApplication(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(projectId, userAccount, cancellationToken);
        var projectSite = await _siteRepository.GetProjectSites(projectId, userAccount, cancellationToken);

        if (project.IsProjectValidForLoanApplication() && projectSite.AreSitesValidForLoanApplication())
        {
            return ApplicationType.Loans;
        }

        return ApplicationType.Undefined;
    }
}
