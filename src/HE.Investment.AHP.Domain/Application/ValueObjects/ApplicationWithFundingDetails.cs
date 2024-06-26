using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public record ApplicationWithFundingDetails(
    FrontDoorProjectId ProjectId,
    SiteId SiteId,
    AhpApplicationId ApplicationId,
    string ApplicationName,
    string ReferenceNumber,
    ApplicationStatus Status,
    Tenure Tenure,
    int? HousesToDeliver,
    decimal? RequiredFunding,
    decimal? OtherApplicationCosts,
    decimal? CurrentLandValue,
    bool? RepresentationsAndWarranties)
{
    public decimal? TotalSchemeCost()
    {
        return OtherApplicationCosts + CurrentLandValue;
    }
}
