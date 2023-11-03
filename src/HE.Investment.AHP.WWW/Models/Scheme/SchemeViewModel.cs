namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SchemeViewModel(
    string ApplicationId,
    string ApplicationName,
    string SchemeId,
    decimal? RequiredFunding,
    int? HousesToDeliver,
    string AffordabilityEvidence,
    string SalesRisk,
    string TypeAndTenureJustification,
    string SchemeAndProposalJustification);
