using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveHomeOwnershipDisabilitiesCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : ISaveHomeTypeSegmentCommand;
