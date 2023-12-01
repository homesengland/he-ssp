using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public class ApplicationSummaryViewModelFactory : IApplicationSummaryViewModelFactory
{
    private readonly IMediator _mediator;
    private readonly ISchemeSummaryViewModelFactory _schemeSummaryViewModelFactory;

    public ApplicationSummaryViewModelFactory(
        IMediator mediator,
        ISchemeSummaryViewModelFactory schemeSummaryViewModelFactory)
    {
        _mediator = mediator;
        _schemeSummaryViewModelFactory = schemeSummaryViewModelFactory;
    }

    public async Task<ApplicationSummaryViewModel> GetDataAndCreate(string applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        var schemeSummary = _schemeSummaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper);

        return new ApplicationSummaryViewModel(applicationId, scheme.ApplicationName, new List<SectionSummaryViewModel> { schemeSummary });
    }
}
