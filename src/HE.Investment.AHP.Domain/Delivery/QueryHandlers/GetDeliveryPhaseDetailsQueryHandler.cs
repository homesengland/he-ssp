using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
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
                request.IncludeSummary ? GetSummaryOfDelivery(deliveryPhase) : null,
                request.IncludeSummary ? GetSummaryOfDeliveryAmend(deliveryPhase) : null),
            deliveryPhase.Organisation.IsUnregisteredBody,
            deliveryPhase.DeliveryPhaseMilestones.IsOnlyCompletionMilestone,
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.MilestoneDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate?.Value),
            deliveryPhase.IsAdditionalPaymentRequested?.IsRequested);
    }

    private SummaryOfDelivery GetSummaryOfDelivery(IDeliveryPhaseEntity deliveryPhase)
    {
        var milestonesPercentageTranches = deliveryPhase.Tranches.GetPercentageTranches();

        return new SummaryOfDelivery(
            deliveryPhase.MilestonesTranches.SumOfGrantApportioned,
            deliveryPhase.MilestonesTranches.AcquisitionMilestone,
            milestonesPercentageTranches.Acquisition?.Value,
            deliveryPhase.MilestonesTranches.StartOnSiteMilestone,
            milestonesPercentageTranches.StartOnSite?.Value,
            deliveryPhase.MilestonesTranches.CompletionMilestone,
            milestonesPercentageTranches.Completion?.Value);
    }

    private SummaryOfDeliveryAmend GetSummaryOfDeliveryAmend(IDeliveryPhaseEntity deliveryPhase)
    {
        var tranches = deliveryPhase.Tranches;
        var milestonesPercentageTranches = tranches.GetPercentageTranches();
        var milestonesTranches = tranches.CalculateTranches();

        return new SummaryOfDeliveryAmend(
            tranches.GrantApportioned,
            milestonesTranches.AcquisitionMilestone,
            milestonesPercentageTranches.Acquisition?.Value.ToWholePercentage().WithoutPercentageChar(),
            milestonesTranches.StartOnSiteMilestone,
            milestonesPercentageTranches.StartOnSite?.Value.ToWholePercentage().WithoutPercentageChar(),
            milestonesTranches.CompletionMilestone,
            milestonesPercentageTranches.Completion?.Value.ToWholePercentage().WithoutPercentageChar(),
            deliveryPhase.Tranches.ClaimMilestone);
    }
}
