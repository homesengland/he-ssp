namespace HE.Investment.AHP.Contract.Delivery.MilestonePayments;

public record SummaryOfDeliveryAmend(
    decimal? GrantApportioned,
    decimal? AcquisitionMilestone,
    decimal? AcquisitionPercentage,
    decimal? StartOnSiteMilestone,
    decimal? StartOnSitePercentage,
    decimal? CompletionMilestone,
    decimal? CompletionPercentage,
    bool? UnderstandClaimingMilestones = null);
