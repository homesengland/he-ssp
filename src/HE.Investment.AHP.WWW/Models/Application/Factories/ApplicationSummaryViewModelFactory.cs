using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Models.HomeTypes.Factories;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public class ApplicationSummaryViewModelFactory : IApplicationSummaryViewModelFactory
{
    private readonly IMediator _mediator;
    private readonly ISchemeSummaryViewModelFactory _schemeSummaryViewModelFactory;
    private readonly IHomeTypeSummaryViewModelFactory _homeTypeSummaryViewModelFactory;
    private readonly IFinancialDetailsSummaryViewModelFactory _financialDetailsSummaryViewModelFactory;

    public ApplicationSummaryViewModelFactory(
        IMediator mediator,
        ISchemeSummaryViewModelFactory schemeSummaryViewModelFactory,
        IHomeTypeSummaryViewModelFactory homeTypeSummaryViewModelFactory,
        IFinancialDetailsSummaryViewModelFactory financialDetailsSummaryViewModelFactory)
    {
        _mediator = mediator;
        _schemeSummaryViewModelFactory = schemeSummaryViewModelFactory;
        _homeTypeSummaryViewModelFactory = homeTypeSummaryViewModelFactory;
        _financialDetailsSummaryViewModelFactory = financialDetailsSummaryViewModelFactory;
    }

    public async Task<ApplicationSummaryViewModel> GetDataAndCreate(string applicationId, IUrlHelper urlHelper, bool isReadOnly, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);
        var schemeSummary = _schemeSummaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper, isReadOnly);
        var homeTypesSummaries = await GetHomeTypesAndCreateSummary(applicationId, urlHelper, isReadOnly, cancellationToken);
        var financialDetailsSummary = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(applicationId, urlHelper, isReadOnly, cancellationToken);

        var summaries = new List<SectionSummaryViewModel> { schemeSummary };
        summaries.AddRange(homeTypesSummaries);
        summaries.Add(financialDetailsSummary.LandValueSummary);
        summaries.Add(financialDetailsSummary.CostsSummary);
        summaries.Add(financialDetailsSummary.ContributionsSummary);

        return new ApplicationSummaryViewModel(applicationId, scheme.ApplicationName, summaries);
    }

    private async Task<IList<SectionSummaryViewModel>> GetHomeTypesAndCreateSummary(string applicationId, IUrlHelper urlHelper, bool isReadOnly, CancellationToken cancellationToken)
    {
        var sections = new List<SectionSummaryViewModel>();
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(applicationId), cancellationToken);
        foreach (var homeType in homeTypes.HomeTypes)
        {
            var fullHomeType = await _mediator.Send(new GetFullHomeTypeQuery(applicationId, homeType.Id), cancellationToken);
            sections.AddRange(_homeTypeSummaryViewModelFactory.CreateSummaryModel(fullHomeType, urlHelper, isReadOnly));
        }

        return sections;
    }
}
