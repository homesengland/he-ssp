using HE.Investments.Common.WWW.Attributes;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SchemeViewModel(
    string ApplicationId,
    string ApplicationName,
    [ErrorSummaryOrder(2)] string? RequiredFunding,
    [ErrorSummaryOrder(1)] string? HousesToDeliver,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? MeetingLocalPriorities,
    string? MeetingLocalHousingNeed,
    string? StakeholderDiscussionsReport,
    IList<FileModel>? UploadedLocalAuthoritySupportFiles,
    int MaxFileSizeInMegabytes,
    string? AllowedExtensions,
    IFormFile? LocalAuthoritySupportFile = null);
