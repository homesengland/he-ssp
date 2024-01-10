using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateSharedOwnershipQuery(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
