using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideSiteNameCommandHandler : IRequestHandler<ProvideSiteNameCommand, OperationResult>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public ProvideSiteNameCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    {
        _siteRepository = siteRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ProvideSiteNameCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projectSites = await _siteRepository.GetSites(request.ProjectId, userAccount, cancellationToken);

        var existingProject = projectSites.ChangeSiteName(request.SiteId, new SiteName(request.Name ?? string.Empty));
        await _siteRepository.Save(existingProject, userAccount, cancellationToken);

        return OperationResult.Success();
    }
}
