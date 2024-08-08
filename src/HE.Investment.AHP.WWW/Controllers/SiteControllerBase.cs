using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investments.Common.WWW.Routing;
using MediatR;

namespace HE.Investment.AHP.WWW.Controllers;

public abstract class SiteControllerBase<TState> : WorkflowController<TState>
    where TState : struct, Enum
{
    private readonly IMediator _mediator;

    protected SiteControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<SiteModel> GetSiteDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteModel.Name;
        ViewBag.ProjectId = siteModel.ProjectId;
        return siteModel;
    }

    protected async Task<SiteBasicModel> GetSiteBasicDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteBasicModel = await _mediator.Send(new GetSiteBasicDetailsQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteBasicModel.Name;
        ViewBag.ProjectId = siteBasicModel.FrontDoorProjectId.Value;
        return siteBasicModel;
    }
}
