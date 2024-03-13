using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateHomeOwnershipDisabilitiesQuery(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? MarketValue,
        string? InitialSale,
        string? RentPerWeek)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
