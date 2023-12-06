using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record TenureDetails(
    string ApplicationName,
    string HomeTypeName,
    int? HomeMarketValue,
    decimal? HomeWeeklyRent,
    decimal? AffordableWeeklyRent,
    decimal? CalculatedPercentage,
    YesNoType TargetRentExceedMarketRent,
    YesNoType ExemptFromTheRightToSharedOwnership);
