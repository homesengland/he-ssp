using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Services.Strategies;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Shared.Project;
using Microsoft.Extensions.Logging;

namespace HE.Investments.FrontDoor.Domain.Services;

public class EligibilityService : IEligibilityService
{
    private readonly IProjectRepository _projectRepository;

    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ILogger _logger;

    private readonly IList<IProjectConversionStrategy> _strategies;

    public EligibilityService(
        IProjectRepository projectRepository,
        IAccountUserContext accountUserContext,
        ISiteRepository siteRepository,
        ILogger<EligibilityService> logger,
        IList<IProjectConversionStrategy> strategies)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _logger = logger;
        _strategies = strategies;
    }

    public async Task<(OperationResult OperationResult, ApplicationType ApplicationType)> GetEligibleApplication(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(projectId, userAccount, cancellationToken);
        var projectSites = await _siteRepository.GetProjectSites(projectId, userAccount, cancellationToken);

        try
        {
            project.CanBeCompleted();
            if (project.IsSiteIdentified?.Value == true)
            {
                if (!projectSites.Sites.Any())
                {
                    OperationResult.New()
                        .AddValidationError("IsSectionCompleted", ValidationErrorMessage.ProvideAllSiteAnswers)
                        .CheckErrors();
                }

                projectSites.AllSitesAreFilled();
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return (domainValidationException.OperationResult, ApplicationType.Undefined);
        }

        var targetApplication = _strategies
            .Select(x => x.Apply(project, projectSites))
            .FirstOrDefault(x => x != ApplicationType.Undefined);

        return (OperationResult.Success(), targetApplication);
    }
}
