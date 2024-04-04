using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveRentToBuyCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    string? MarketValue,
    string? MarketRentPerWeek,
    string? RentPerWeek,
    YesNoType TargetRentExceedMarketRent) : ISaveHomeTypeSegmentCommand;
