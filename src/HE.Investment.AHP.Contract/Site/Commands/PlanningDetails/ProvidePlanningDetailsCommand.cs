using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;

public record ProvidePlanningDetailsCommand(
        SiteId SiteId,
        string? ReferenceNumber,
        bool IsDetailedPlanningApprovalDateActive,
        DateDetails DetailedPlanningApprovalDate,
        string? RequiredFurtherSteps,
        bool IsApplicationForDetailedPlanningSubmittedDateActive,
        DateDetails ApplicationForDetailedPlanningSubmittedDate,
        bool IsExpectedPlanningApprovalDateActive,
        DateDetails ExpectedPlanningApprovalDate,
        bool IsOutlinePlanningApprovalDateActive,
        DateDetails OutlinePlanningApprovalDate,
        bool? IsGrantFundingForAllHomesCoveredByApplication,
        bool IsPlanningSubmissionDateActive,
        DateDetails PlanningSubmissionDate,
        bool? IsLandRegistryTitleNumberRegistered)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
