using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Contract.Application;

public record ApplicationExtendedDetails(
    SiteId SiteId,
    AhpApplicationId ApplicationId,
    string ApplicationName,
    string ReferenceNumber,
    string Tenure,
    int? NumberOfHomes,
    decimal? FundingRequested,
    decimal? TotalSchemeCost,
    bool? RepresentationsAndWarranties);
