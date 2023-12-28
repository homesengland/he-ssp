namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveOlderPersonsSharedOwnershipCommand(
        string ApplicationId,
        string HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
