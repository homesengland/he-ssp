using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public abstract class PlanningDetails : ValueObject, IQuestion
{
    protected PlanningDetails(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ApplicationForDetailedPlanningSubmittedDate? applicationForDetailedPlanningSubmittedDate = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        bool? isGrantFundingForAllHomesCoveredByApplication = null,
        PlanningSubmissionDate? planningSubmissionDate = null,
        LandRegistryDetails? landRegistryDetails = null)
    {
        ReferenceNumber = referenceNumber;
        DetailedPlanningApprovalDate = detailedPlanningApprovalDate;
        RequiredFurtherSteps = requiredFurtherSteps;
        ApplicationForDetailedPlanningSubmittedDate = applicationForDetailedPlanningSubmittedDate;
        ExpectedPlanningApprovalDate = expectedPlanningApprovalDate;
        OutlinePlanningApprovalDate = outlinePlanningApprovalDate;
        IsGrantFundingForAllHomesCoveredByApplication = isGrantFundingForAllHomesCoveredByApplication;
        PlanningSubmissionDate = planningSubmissionDate;
        LandRegistryDetails = landRegistryDetails;
    }

    public abstract SitePlanningStatus? PlanningStatus { get; }

    public ReferenceNumber? ReferenceNumber { get; }

    public DetailedPlanningApprovalDate? DetailedPlanningApprovalDate { get; }

    public RequiredFurtherSteps? RequiredFurtherSteps { get; }

    public ApplicationForDetailedPlanningSubmittedDate? ApplicationForDetailedPlanningSubmittedDate { get; }

    public ExpectedPlanningApprovalDate? ExpectedPlanningApprovalDate { get; }

    public OutlinePlanningApprovalDate? OutlinePlanningApprovalDate { get; }

    public bool? IsGrantFundingForAllHomesCoveredByApplication { get; }

    public PlanningSubmissionDate? PlanningSubmissionDate { get; }

    public LandRegistryDetails? LandRegistryDetails { get; }

    public abstract bool IsAnswered();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return PlanningStatus;
        yield return ReferenceNumber;
        yield return DetailedPlanningApprovalDate;
        yield return RequiredFurtherSteps;
        yield return ApplicationForDetailedPlanningSubmittedDate;
        yield return ExpectedPlanningApprovalDate;
        yield return OutlinePlanningApprovalDate;
        yield return IsGrantFundingForAllHomesCoveredByApplication;
        yield return PlanningSubmissionDate;
        yield return LandRegistryDetails;
    }
}
