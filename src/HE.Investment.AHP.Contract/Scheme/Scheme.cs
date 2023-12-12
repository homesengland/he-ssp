using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Contract.Scheme;

public record Scheme(
    string ApplicationId,
    string ApplicationName,
    Tenure? ApplicationTenure,
    SectionStatus Status,
    decimal? RequiredFunding,
    int? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? MeetingLocalPriorities,
    string? MeetingLocalHousingNeed,
    string? StakeholderDiscussionsReport,
    UploadedFile? StakeholderDiscussionsFile);
