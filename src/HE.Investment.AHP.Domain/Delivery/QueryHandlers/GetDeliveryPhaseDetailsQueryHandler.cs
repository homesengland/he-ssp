using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;
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
                GetSummaryOfDelivery(deliveryPhase)),
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

    private static SummaryOfDelivery GetSummaryOfDelivery(IDeliveryPhaseEntity deliveryPhase)
    {
        return new SummaryOfDelivery(
            deliveryPhase.MilestonesTranches.SumOfGrantApportioned,
            deliveryPhase.MilestonesTranches.AcquisitionMilestone,
            deliveryPhase.Tranches.Percentages.Acquisition?.Value,
            deliveryPhase.MilestonesTranches.StartOnSiteMilestone,
            deliveryPhase.Tranches.Percentages.StartOnSite?.Value,
            deliveryPhase.MilestonesTranches.CompletionMilestone,
            deliveryPhase.Tranches.Percentages.Completion?.Value,
            deliveryPhase.Tranches.ClaimMilestone);
    }
}
