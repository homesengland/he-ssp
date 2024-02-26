using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public record ApplicationWithFundingDetails(
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
    string? RepresentationsAndWarranties)
{
    public decimal? TotalSchemeCost()
    {
        return OtherApplicationCosts + CurrentLandValue;
    }
}
