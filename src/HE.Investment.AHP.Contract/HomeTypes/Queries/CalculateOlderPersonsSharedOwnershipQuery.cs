namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateOlderPersonsSharedOwnershipQuery(
        string ApplicationId,
        string HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
