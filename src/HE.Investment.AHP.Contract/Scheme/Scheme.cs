using HE.Investment.AHP.Contract.Application;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Contract.Scheme;

public record Scheme(
    string ApplicationId,
    string ApplicationName,
    Tenure? ApplicationTenure,
    bool IsCompleted,
    decimal? RequiredFunding,
    int? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? TypeAndTenureJustification,
    string? SchemeAndProposalJustification,
    string? StakeholderDiscussionsReport,
    UploadedFile? StakeholderDiscussionsFile);
