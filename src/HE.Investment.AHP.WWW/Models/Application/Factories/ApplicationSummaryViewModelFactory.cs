using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public class ApplicationSummaryViewModelFactory : IApplicationSummaryViewModelFactory
{
    private readonly IMediator _mediator;
    private readonly ISchemeSummaryViewModelFactory _schemeSummaryViewModelFactory;
    private readonly IFinancialDetailsSummaryViewModelFactory _financialDetailsSummaryViewModelFactory;

    public ApplicationSummaryViewModelFactory(
        IMediator mediator,
        ISchemeSummaryViewModelFactory schemeSummaryViewModelFactory,
        IFinancialDetailsSummaryViewModelFactory financialDetailsSummaryViewModelFactory)
    {
        _mediator = mediator;
        _schemeSummaryViewModelFactory = schemeSummaryViewModelFactory;
        _financialDetailsSummaryViewModelFactory = financialDetailsSummaryViewModelFactory;
    }

    public async Task<ApplicationSummaryViewModel> GetDataAndCreate(string applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        var schemeSummary = _schemeSummaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper);
        var financialDetailsSummary = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(applicationId, urlHelper, cancellationToken);

        var summaries = new List<SectionSummaryViewModel>
        {
            schemeSummary,
            financialDetailsSummary.LandValueSummary,
            financialDetailsSummary.CostsSummary,
            financialDetailsSummary.ContributionsSummary,
        };

        return new ApplicationSummaryViewModel(applicationId, scheme.ApplicationName, summaries);
    }
}
