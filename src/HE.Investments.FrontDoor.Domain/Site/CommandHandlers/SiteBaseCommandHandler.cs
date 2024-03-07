using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class SiteBaseCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult>
    where TRequest : IProvideSiteDetailsCommand
{
    public SiteBaseCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    {
        SiteRepository = siteRepository;
        AccountUserContext = accountUserContext;
    }

    protected ISiteRepository SiteRepository { get; }

    protected IAccountUserContext AccountUserContext { get; }

    public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var userAccount = await AccountUserContext.GetSelectedAccount();
        var projectSite = await SiteRepository.GetSite(request.ProjectId, request.SiteId, userAccount, cancellationToken);

        Perform(projectSite, request);
        await PerformAsync(projectSite, request, cancellationToken);

        await SiteRepository.Save(projectSite, userAccount, cancellationToken);
        return OperationResult.Success();
    }

    protected virtual void Perform(ProjectSiteEntity projectSite, TRequest request)
    {
    }

    protected virtual Task PerformAsync(ProjectSiteEntity projectSite, TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
