using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class DeliveryPhaseTranches : IQuestion
{
    public DeliveryPhaseTranches(DeliveryPhaseId id, ApplicationBasicInfo applicationBasicInfo, MilestoneTranches milestoneTranches, bool allowAmendments)
    {
        Id = id;
        MilestoneTranches = milestoneTranches;
        AllowAmendments = allowAmendments;
        ApplicationBasicInfo = applicationBasicInfo;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestoneTranches MilestoneTranches { get; private set; }

    public bool AllowAmendments { get; }

    public void ProvideAcquisitionTranche(decimal? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = MilestoneTranches.WithAcquisition(acquisition);
    }

    public void ProvideStartOnSiteTranche(decimal? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = MilestoneTranches.WithStartOnSite(acquisition);
    }

    public void ProvideCompletionTranche(decimal? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = MilestoneTranches.WithCompletion(acquisition);
    }

    public SummaryOfDelivery CalculateSummary(
        decimal requiredFunding,
        int totalHousesToDeliver,
        int totalHomesToBeDeliveredInThisPhase,
        bool isOneTranche,
        MilestoneFramework milestoneFramework)
    {
        if (requiredFunding <= 0 || totalHousesToDeliver <= 0 || totalHomesToBeDeliveredInThisPhase <= 0)
        {
            return SummaryOfDelivery.LackOfCalculation;
        }

        var grantApportioned = requiredFunding * totalHomesToBeDeliveredInThisPhase / totalHousesToDeliver;
        if (MilestoneTranches.IsAmended)
        {
            return new SummaryOfDelivery(grantApportioned, MilestoneTranches.Acquisition, MilestoneTranches.StartOnSite, MilestoneTranches.Completion, false);
        }

        if (isOneTranche)
        {
            return new SummaryOfDelivery(grantApportioned, null, null, grantApportioned);
        }

        var acquisitionMilestone = (grantApportioned * milestoneFramework.AcquisitionPercentage).ToWholeNumberRoundFloor();
        var startOnSiteMilestone = (grantApportioned * milestoneFramework.StartOnSitePercentage).ToWholeNumberRoundFloor();
        var completionMilestone = grantApportioned - acquisitionMilestone - startOnSiteMilestone;

        return new SummaryOfDelivery(grantApportioned, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
    }

    public bool IsAnswered()
    {
        if (!AllowAmendments)
        {
            return true;
        }

        return MilestoneTranches.IsAnswered();
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!AllowAmendments || ApplicationBasicInfo.Status.IsNotIn(ApplicationStatus.ReferredBackToApplicant))
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amendment");
        }
    }
}
