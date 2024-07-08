using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record MilestoneClaim(
    string Status,
    decimal AmountOfGrantApportioned,
    decimal PercentageOfGrantApportioned,
    DateDetails ForecastClaimDate,
    DateDetails? ClaimDate);
