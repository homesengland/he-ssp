using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateRentToBuyQuery(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? MarketValue,
        string? MarketRentPerWeek,
        string? RentPerWeek,
        YesNoType TargetRentExceedMarketRent)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
