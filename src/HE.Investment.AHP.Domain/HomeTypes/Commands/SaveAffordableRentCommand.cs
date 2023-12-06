using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveAffordableRentCommand(
    string ApplicationId,
    string HomeTypeId,
    string? HomeMarketValue,
    string? HomeWeeklyRent,
    string? AffordableWeeklyRent,
    YesNoType TargetRentExceedMarketRent) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
