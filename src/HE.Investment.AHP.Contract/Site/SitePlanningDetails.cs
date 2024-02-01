using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public record SitePlanningDetails(
    SitePlanningStatus? PlanningStatus,
    string? ReferenceNumber = null,
    DateDetails? DetailedPlanningApprovalDate = null,
    string? RequiredFurtherSteps = null,
    DateDetails? ApplicationForDetailedPlanningSubmittedDate = null,
    DateDetails? ExpectedPlanningApprovalDate = null,
    DateDetails? OutlinePlanningApprovalDate = null,
    bool? IsGrantFundingForAllHomesCoveredByApplication = null,
    DateDetails? PlanningSubmissionDate = null,
    bool? IsLandRegistryTitleNumberRegistered = null,
    string? LandRegistryTitleNumber = null,
    bool? IsGrantFundingForAllHomesCoveredByTitleNumber = null,
    bool ArePlanningDetailsProvided = false);
