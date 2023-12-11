namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveSharedOwnershipCommand(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? InitialSalePercentage,
    string? SharedOwnershipWeeklyRent) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
