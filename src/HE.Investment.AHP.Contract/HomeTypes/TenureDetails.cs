using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record TenureDetails(
    string ApplicationName,
    string HomeTypeName,
    int? MarketValue,
    decimal? MarketRent,
    decimal? ProspectiveRent,
    decimal? ProspectiveRentAsPercentageOfMarketRent,
    YesNoType TargetRentExceedMarketRent,
    YesNoType ExemptFromTheRightToSharedOwnership,
    string? ExemptionJustification,
    bool IsProspectiveRentIneligible,
    decimal? InitialSale,
    decimal? ExpectedFirstTranche,
    decimal? RentAsPercentageOfTheUnsoldShare)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
