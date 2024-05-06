using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.Site;

public record SitePlanningDetails(
    SitePlanningStatus? PlanningStatus,
    string? ReferenceNumber = null,
    bool IsReferenceNumberActive = false,
    DateDetails? DetailedPlanningApprovalDate = null,
    bool IsDetailedPlanningApprovalDateActive = false,
    string? RequiredFurtherSteps = null,
    bool IsRequiredFurtherStepsActive = false,
    DateDetails? ApplicationForDetailedPlanningSubmittedDate = null,
    bool IsApplicationForDetailedPlanningSubmittedDateActive = false,
    DateDetails? ExpectedPlanningApprovalDate = null,
    bool IsExpectedPlanningApprovalDateActive = false,
    DateDetails? OutlinePlanningApprovalDate = null,
    bool IsOutlinePlanningApprovalDateActive = false,
    bool? IsGrantFundingForAllHomesCoveredByApplication = null,
    bool IsGrantFundingForAllHomesCoveredByApplicationActive = false,
    DateDetails? PlanningSubmissionDate = null,
    bool IsPlanningSubmissionDateActive = false,
    bool? IsLandRegistryTitleNumberRegistered = null,
    string? LandRegistryTitleNumber = null,
    bool? IsGrantFundingForAllHomesCoveredByTitleNumber = null,
    bool IsLandRegistryActive = false,
    bool ArePlanningDetailsProvided = false,
    string? LocalAuthorityCode = null);
