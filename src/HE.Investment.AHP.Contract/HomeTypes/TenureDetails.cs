using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record TenureDetails(
    string ApplicationName,
    string HomeTypeName,
    int? MarketValue,
    decimal? MarketRentPerWeek,
    decimal? RentPerWeek,
    decimal? ProspectiveRentAsPercentageOfMarketRent,
    YesNoType TargetRentExceedMarketRent,
    YesNoType ExemptFromTheRightToSharedOwnership,
    string? ExemptionJustification,
    bool IsProspectiveRentIneligible,
    decimal? InitialSale,
    decimal? ExpectedFirstTranche,
    decimal? RentAsPercentageOfTheUnsoldShare)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
