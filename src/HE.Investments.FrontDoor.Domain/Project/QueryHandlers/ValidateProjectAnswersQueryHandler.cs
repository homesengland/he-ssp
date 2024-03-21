using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class ValidateProjectAnswersQueryHandler : IRequestHandler<ValidateProjectAnswersQuery, ApplicationType>
{
    private readonly IProjectRepository _projectRepository;

    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public ValidateProjectAnswersQueryHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<ApplicationType> Handle(ValidateProjectAnswersQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(request.ProjectId, userAccount, cancellationToken);
        var projectSite = await _siteRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);

        if (project.IsProjectValidForLoanApplication() && projectSite.AreSitesValidForLoanApplication())
        {
            return ApplicationType.Loans;
        }

        return ApplicationType.Undefined;
    }
}
