using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record TenureDetails(
    string ApplicationName,
    string HomeTypeName,
    int? MarketValue,
    decimal? MarketRent,
    decimal? ProspectiveRent,
    decimal? CalculatedProspectivePercentage,
    YesNoType TargetRentExceedMarketRent,
    YesNoType ExemptFromTheRightToSharedOwnership,
    string? ExemptionJustification,
    bool IsExceeding80PercentOfMarketRent,
    decimal? InitialSalePercentage,
    decimal? ExpectedFirstTranche,
    decimal? SharedOwnershipRentAsPercentageOfTheUnsoldShare,
    bool IsExceeding3PercentOfUnsoldShare)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
