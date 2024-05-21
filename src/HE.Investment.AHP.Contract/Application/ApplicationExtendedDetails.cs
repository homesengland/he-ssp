using HE.Investment.AHP.Contract.Site;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Application;

public record ApplicationExtendedDetails(
    FrontDoorProjectId ProjectId,
    SiteId SiteId,
    AhpApplicationId ApplicationId,
    string ApplicationName,
    string ReferenceNumber,
    string Tenure,
    int? NumberOfHomes,
    decimal? FundingRequested,
    decimal? TotalSchemeCost,
    bool? RepresentationsAndWarranties);
