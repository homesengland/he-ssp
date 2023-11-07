namespace HE.Investment.AHP.Contract.Scheme;

public record Scheme(
    decimal? RequiredFunding,
    int? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? TypeAndTenureJustification,
    string? SchemeAndProposalJustification);
