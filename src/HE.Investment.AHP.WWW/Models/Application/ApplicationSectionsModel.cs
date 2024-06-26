using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationSectionsModel(
    string ApplicationId,
    string ProjectId,
    string SiteName,
    string Name,
    ApplicationStatus Status,
    IReadOnlyCollection<AhpApplicationOperation> AllowedOperations,
    string? ReferenceNumber,
    ModificationDetails? LastModificationDetails,
    ModificationDetails? LastSubmissionDetails,
    IList<ApplicationSection> Sections);
