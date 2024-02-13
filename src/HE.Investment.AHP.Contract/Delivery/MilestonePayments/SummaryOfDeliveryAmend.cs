namespace HE.Investment.AHP.Contract.Delivery.MilestonePayments;

public record SummaryOfDeliveryAmend(
    decimal? GrantApportioned,
    decimal? AcquisitionMilestone,
    string? AcquisitionPercentage,
    decimal? StartOnSiteMilestone,
    string? StartOnSitePercentage,
    decimal? CompletionMilestone,
    string? CompletionPercentage,
    bool? UnderstandClaimingMilestones = null);
