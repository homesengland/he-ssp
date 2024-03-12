using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveAffordableRentCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    string? MarketValue,
    string? MarketRentPerWeek,
    string? AffordableRentPerWeek,
    YesNoType TargetRentExceedMarketRent) : ISaveHomeTypeSegmentCommand;
