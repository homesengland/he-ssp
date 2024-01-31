using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;

public record ProvidePlanningDetailsCommand(
        SiteId SiteId,
        string? ReferenceNumber,
        DateDetails DetailedPlanningApprovalDate,
        string? RequiredFurtherSteps,
        DateDetails ApplicationForDetailedPlanningSubmittedDate,
        DateDetails ExpectedPlanningApprovalDate,
        DateDetails OutlinePlanningApprovalDate,
        bool? IsGrantFundingForAllHomesCoveredByApplication,
        DateDetails PlanningSubmissionDate,
        bool? IsLandRegistryTitleNumberRegistered)
    : IRequest<OperationResult>, IProvideSitePlanningDetailsCommand;
