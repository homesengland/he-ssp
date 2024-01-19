using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateSharedOwnershipQuery(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? ProspectiveRent)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
