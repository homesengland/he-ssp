using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public record SitePlanningDetails(
    SitePlanningStatus? PlanningStatus,
    string? ReferenceNumber,
    DateDetails? DetailedPlanningApprovalDate,
    string? RequiredFurtherSteps,
    DateDetails? ApplicationForDetailedPlanningSubmittedDate,
    DateDetails? ExpectedPlanningApprovalDate,
    DateDetails? OutlinePlanningApprovalDate,
    bool? IsGrantFundingForAllHomes,
    DateDetails? PlanningSubmissionDate,
    bool? IsLandRegistryTitleNumberRegistered,
    string? LandRegistryTitleNumber);
