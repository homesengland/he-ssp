namespace HE.Investment.AHP.Contract.Delivery.MilestonePayments;

public record SummaryOfDelivery(
    decimal? GrantApportioned,
    decimal? AcquisitionMilestone,
    decimal? AcquisitionPercentage,
    decimal? StartOnSiteMilestone,
    decimal? StartOnSitePercentage,
    decimal? CompletionMilestone,
    decimal? CompletionPercentage);
