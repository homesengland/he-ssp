using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateProspectiveRentQuery(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        string? MarketValue,
        string? MarketRent,
        string? ProspectiveRent,
        YesNoType TargetRentExceedMarketRent)
    : CalculateQueryBase(ApplicationId, HomeTypeId);
