using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Services.Strategies;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.UtilsService.BannerNotification.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investments.FrontDoor.Domain.Services;

public class EligibilityService : IEligibilityService
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ILogger _logger;

    private readonly IList<IProjectConversionStrategy> _strategies;

    public EligibilityService(
        IAccountUserContext accountUserContext,
        ISiteRepository siteRepository,
        ILogger<EligibilityService> logger,
        IList<IProjectConversionStrategy> strategies)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _logger = logger;
        _strategies = strategies;
    }

    public async Task<(OperationResult OperationResult, ApplicationType ApplicationType)> GetEligibleApplication(ProjectEntity project, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projectSites = await _siteRepository.GetProjectSites(project.Id, userAccount, cancellationToken);

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

        var targetApplication = ApplicationType.Undefined;

        foreach (var strategy in _strategies)
        {
            targetApplication = await strategy.Apply(project, projectSites, cancellationToken);
            if (targetApplication != ApplicationType.Undefined)
            {
                break;
            }
        }

        return (OperationResult.Success(), targetApplication);
    }
}
