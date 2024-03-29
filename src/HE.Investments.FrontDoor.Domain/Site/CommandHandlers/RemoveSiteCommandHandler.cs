using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class RemoveSiteCommandHandler : IRequestHandler<RemoveSiteCommand, OperationResult>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IRemoveSiteRepository _removeSiteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public RemoveSiteCommandHandler(ISiteRepository siteRepository, IRemoveSiteRepository removeSiteRepository, IAccountUserContext accountUserContext)
    {
        _siteRepository = siteRepository;
        _removeSiteRepository = removeSiteRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(RemoveSiteCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projectSites = await _siteRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);
        await projectSites.RemoveSite(_removeSiteRepository, request.SiteId, userAccount, request.RemoveSiteAnswer, cancellationToken);

        return OperationResult.Success();
    }
}
