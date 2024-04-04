using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.WWW.Models.Delivery.Factories;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Models.HomeTypes.Factories;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using HE.Investments.Common.WWW.Models.Summary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public class ApplicationSummaryViewModelFactory : IApplicationSummaryViewModelFactory
{
    private readonly IMediator _mediator;
    private readonly ISchemeSummaryViewModelFactory _schemeSummaryViewModelFactory;
    private readonly IHomeTypeSummaryViewModelFactory _homeTypeSummaryViewModelFactory;
    private readonly IFinancialDetailsSummaryViewModelFactory _financialDetailsSummaryViewModelFactory;
    private readonly IDeliveryPhaseCheckAnswersViewModelFactory _deliveryPhaseCheckAnswersViewModelFactory;

    public ApplicationSummaryViewModelFactory(
        IMediator mediator,
        ISchemeSummaryViewModelFactory schemeSummaryViewModelFactory,
        IHomeTypeSummaryViewModelFactory homeTypeSummaryViewModelFactory,
        IFinancialDetailsSummaryViewModelFactory financialDetailsSummaryViewModelFactory,
        IDeliveryPhaseCheckAnswersViewModelFactory deliveryPhaseCheckAnswersViewModelFactory)
    {
        _mediator = mediator;
        _schemeSummaryViewModelFactory = schemeSummaryViewModelFactory;
        _homeTypeSummaryViewModelFactory = homeTypeSummaryViewModelFactory;
        _financialDetailsSummaryViewModelFactory = financialDetailsSummaryViewModelFactory;
        _deliveryPhaseCheckAnswersViewModelFactory = deliveryPhaseCheckAnswersViewModelFactory;
    }

    public async Task<ApplicationSummaryViewModel> GetDataAndCreate(AhpApplicationId applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);
        var schemeSummary = _schemeSummaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper);
        var homeTypesSummaries = await GetHomeTypesAndCreateSummary(applicationId, urlHelper, cancellationToken);
        var financialDetailsSummary = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(applicationId, urlHelper, cancellationToken);
        var deliveryPhasesSummaries = await GetDeliveryPhasesAndCreateSummary(applicationId, urlHelper, cancellationToken);

        var summaries = new List<SectionSummaryViewModel> { schemeSummary };
        summaries.AddRange(homeTypesSummaries);
        summaries.Add(financialDetailsSummary.LandValueSummary);
        summaries.Add(financialDetailsSummary.CostsSummary);
        summaries.Add(financialDetailsSummary.ContributionsSummary);
        summaries.AddRange(deliveryPhasesSummaries);

        return new ApplicationSummaryViewModel(applicationId.Value, scheme.Application.Name, summaries);
    }

    private async Task<IList<SectionSummaryViewModel>> GetHomeTypesAndCreateSummary(AhpApplicationId applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var sections = new List<SectionSummaryViewModel>();
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(applicationId), cancellationToken);
        foreach (var homeType in homeTypes.HomeTypes)
        {
            var fullHomeType = await _mediator.Send(new GetFullHomeTypeQuery(applicationId, homeType.Id), cancellationToken);
            sections.AddRange(_homeTypeSummaryViewModelFactory.CreateSummaryModel(fullHomeType, urlHelper, true));
        }

        return sections;
    }

    private async Task<IList<SectionSummaryViewModel>> GetDeliveryPhasesAndCreateSummary(AhpApplicationId applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var sections = new List<SectionSummaryViewModel>();
        var deliveryPhases = await _mediator.Send(new GetDeliveryPhasesQuery(applicationId), cancellationToken);
        foreach (var deliveryPhaseId in deliveryPhases.DeliveryPhases.Select(x => new DeliveryPhaseId(x.Id)))
        {
            var deliveryPhaseDetails = await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId, true), cancellationToken);
            var deliveryPhaseHomes = await _mediator.Send(new GetDeliveryPhaseHomesQuery(applicationId, deliveryPhaseId), cancellationToken);

            sections.AddRange(_deliveryPhaseCheckAnswersViewModelFactory.CreateSummary(applicationId, deliveryPhaseDetails, deliveryPhaseHomes, urlHelper));
        }

        return sections;
    }
}
