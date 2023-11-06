namespace HE.Investment.AHP.Contract.Scheme;

public record Scheme(
    string SchemeId,
    decimal? RequiredFunding,
    int? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk);
