using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveProspectiveRentCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    string? MarketValue,
    string? MarketRent,
    string? ProspectiveRent,
    YesNoType TargetRentExceedMarketRent) : ISaveHomeTypeSegmentCommand;