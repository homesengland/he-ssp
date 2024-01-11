using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveOlderPersonsSharedOwnershipCommand(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : ISaveHomeTypeSegmentCommand;
