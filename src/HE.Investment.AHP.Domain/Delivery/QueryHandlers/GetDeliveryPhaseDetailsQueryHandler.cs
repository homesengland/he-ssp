using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using MediatR;
using SummaryOfDelivery = HE.Investment.AHP.Contract.Delivery.MilestonePayments.SummaryOfDelivery;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhaseDetailsQueryHandler : IRequestHandler<GetDeliveryPhaseDetailsQuery, DeliveryPhaseDetails>
{
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetDeliveryPhaseDetailsQueryHandler(
        IDeliveryPhaseRepository deliveryPhaseRepository,
        IAccountUserContext accountUserContext)
    {
        _deliveryPhaseRepository = deliveryPhaseRepository;
        _accountUserContext = accountUserContext;
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
            deliveryPhase.IsReadOnly,
            deliveryPhase.IsApplicationLocked,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.BuildActivity.Type,
            deliveryPhase.BuildActivity.GetAvailableTypes(),
            deliveryPhase.IsReconfiguringExistingNeeded(),
            deliveryPhase.ReconfiguringExisting,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            new DeliveryPhaseTranchesDto(
                deliveryPhase.Tranches.CanBeAmended,
                request.IncludeSummary ? GetSummaryOfDelivery(deliveryPhase) : null,
                request.IncludeSummary ? GetSummaryOfDeliveryAmend(deliveryPhase) : null),
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

    private SummaryOfDelivery GetSummaryOfDelivery(IDeliveryPhaseEntity deliveryPhase)
    {
        var result = deliveryPhase.GetSummaryOfDelivery();

        return new SummaryOfDelivery(
            result.GrantApportioned,
            result.AcquisitionMilestone,
            result.SummaryOfDeliveryPercentage.AcquisitionPercentage,
            result.StartOnSiteMilestone,
            result.SummaryOfDeliveryPercentage.StartOnSitePercentage,
            result.CompletionMilestone,
            result.SummaryOfDeliveryPercentage.CompletionPercentage);
    }

    private SummaryOfDeliveryAmend GetSummaryOfDeliveryAmend(IDeliveryPhaseEntity deliveryPhase)
    {
        var result = GetSummaryOfDelivery(deliveryPhase);

        return new SummaryOfDeliveryAmend(
            result.GrantApportioned,
            result.AcquisitionMilestone,
            result.AcquisitionPercentage.ToWholePercentage().WithoutPercentageChar(),
            result.StartOnSiteMilestone,
            result.StartOnSitePercentage.ToWholePercentage().WithoutPercentageChar(),
            result.CompletionMilestone,
            result.CompletionPercentage.ToWholePercentage().WithoutPercentageChar());
    }
}
