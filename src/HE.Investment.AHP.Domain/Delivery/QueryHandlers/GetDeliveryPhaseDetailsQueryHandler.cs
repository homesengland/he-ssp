using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
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
            ApplicationBasicInfoMapper.Map(deliveryPhase.Application),
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.Status,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.BuildActivity.Type,
            deliveryPhase.BuildActivity.GetAvailableTypes(),
            deliveryPhase.IsReconfiguringExistingNeeded(),
            deliveryPhase.ReconfiguringExisting,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            new DeliveryPhaseTranchesDto(
                deliveryPhase.Tranches.CanBeAmended,
                GetSummaryOfDelivery(deliveryPhase.Tranches)),
            deliveryPhase.Organisation.IsUnregisteredBody,
            deliveryPhase.IsOnlyCompletionMilestone,
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate?.Value),
            deliveryPhase.IsAdditionalPaymentRequested?.IsRequested);
    }

    private static SummaryOfDelivery GetSummaryOfDelivery(DeliveryPhaseTranches tranches)
    {
        return new SummaryOfDelivery(
            tranches.CalculatedValues.SumOfGrantApportioned,
            tranches.CalculatedValues.AcquisitionMilestone,
            tranches.Percentages.Acquisition?.Value,
            tranches.CalculatedValues.StartOnSiteMilestone,
            tranches.Percentages.StartOnSite?.Value,
            tranches.CalculatedValues.CompletionMilestone,
            tranches.Percentages.Completion?.Value,
            tranches.ClaimMilestone);
    }
}
