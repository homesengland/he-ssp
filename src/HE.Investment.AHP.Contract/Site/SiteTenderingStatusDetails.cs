namespace HE.Investment.AHP.Contract.Site;

public record SiteTenderingStatusDetails(SiteTenderingStatus? TenderingStatus, string? ContractorName, bool? IsSmeContractor, bool? IsIntentionToWorkWithSme);
