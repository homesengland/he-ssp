namespace HE.Investment.AHP.Contract.Delivery;

public record SummaryOfDelivery(
    decimal? GrantApportioned,
    decimal? AcquisitionMilestone,
    decimal? AcquisitionPercentage,
    decimal? StartOnSiteMilestone,
    decimal? StartOnSitePercentage,
    decimal? CompletionMilestone,
    decimal? CompletionPercentage);
