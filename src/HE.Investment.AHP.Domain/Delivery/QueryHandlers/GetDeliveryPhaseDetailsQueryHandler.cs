using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using MediatR;
using SummaryOfDelivery = HE.Investment.AHP.Contract.Delivery.MilestonePayments.SummaryOfDelivery;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhaseDetailsQueryHandler : IRequestHandler<GetDeliveryPhaseDetailsQuery, DeliveryPhaseDetails>
{
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

    private readonly ISchemeRepository _schemeRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetDeliveryPhaseDetailsQueryHandler(
        IDeliveryPhaseRepository deliveryPhaseRepository,
        ISchemeRepository schemeRepository,
        IAccountUserContext accountUserContext)
    {
        _deliveryPhaseRepository = deliveryPhaseRepository;
        _accountUserContext = accountUserContext;
        _schemeRepository = schemeRepository;
    }

    public async Task<DeliveryPhaseDetails> Handle(GetDeliveryPhaseDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var deliveryPhase = await _deliveryPhaseRepository.GetById(
            request.ApplicationId,
            request.DeliveryPhaseId,
            userAccount,
            cancellationToken);

        return new DeliveryPhaseDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.Status,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.BuildActivity.Type,
            deliveryPhase.BuildActivity.GetAvailableTypes(),
            deliveryPhase.IsReconfiguringExistingNeeded(),
            deliveryPhase.ReconfiguringExisting,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            request.IncludeSummary ? await GetSummaryOfDelivery(deliveryPhase, userAccount, cancellationToken) : null,
            request.IncludeSummary ? await GetSummaryOfDeliveryAmend(deliveryPhase, userAccount, cancellationToken) : null,
            deliveryPhase.Organisation.IsUnregisteredBody,
            deliveryPhase.DeliveryPhaseMilestones.IsOnlyCompletionMilestone,
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.MilestoneDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.MilestoneDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.MilestoneDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate),
            deliveryPhase.IsAdditionalPaymentRequested?.IsRequested);
    }

    private async Task<SummaryOfDelivery> GetSummaryOfDelivery(IDeliveryPhaseEntity deliveryPhase, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var schemaInformation = await _schemeRepository.GetByApplicationId(deliveryPhase.ApplicationId, userAccount, false, cancellationToken);

        var result = deliveryPhase.CalculateSummary(
            schemaInformation.Funding.RequiredFunding ?? 0,
            schemaInformation.Funding.HousesToDeliver ?? 0,
            MilestoneFramework.Default);

        return new SummaryOfDelivery(
            result.GrantApportioned,
            result.AcquisitionMilestone,
            result.SummaryOfDeliveryPercentage.AcquisitionPercentage,
            result.StartOnSiteMilestone,
            result.SummaryOfDeliveryPercentage.StartOnSitePercentage,
            result.CompletionMilestone,
            result.SummaryOfDeliveryPercentage.CompletionPercentage);
    }

    private async Task<SummaryOfDeliveryAmend> GetSummaryOfDeliveryAmend(IDeliveryPhaseEntity deliveryPhase, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var result = await GetSummaryOfDelivery(deliveryPhase, userAccount, cancellationToken);

        return new SummaryOfDeliveryAmend(
            result.GrantApportioned,
            result.AcquisitionMilestone,
            result.AcquisitionPercentage,
            result.StartOnSiteMilestone,
            result.StartOnSitePercentage,
            result.CompletionMilestone,
            result.CompletionPercentage);
    }
}
