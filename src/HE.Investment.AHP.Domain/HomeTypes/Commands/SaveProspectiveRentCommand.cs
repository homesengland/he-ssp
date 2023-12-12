using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveProspectiveRentCommand(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? MarketRent,
    string? ProspectiveRent,
    YesNoType TargetRentExceedMarketRent) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
