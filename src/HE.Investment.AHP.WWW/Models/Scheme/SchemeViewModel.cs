using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SchemeViewModel(
    string ApplicationId,
    string ApplicationName,
    string? RequiredFunding,
    string? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? TypeAndTenureJustification,
    string? SchemeAndProposalJustification,
    string? StakeholderDiscussionsReport,
    IList<FileModel>? UploadedStakeholderDiscussionFiles,
    IFormFile? StakeholderDiscussionFile = null);
