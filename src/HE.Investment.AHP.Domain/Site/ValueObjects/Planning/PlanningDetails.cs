using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

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

    protected abstract IReadOnlyCollection<string> ActiveFields { get; }

    public bool IsAnswered()
    {
        return ActiveFields.Any()
               && (!IsQuestionActive(nameof(ReferenceNumber)) || ReferenceNumber.IsProvided())
               && (!IsQuestionActive(nameof(DetailedPlanningApprovalDate)) || DetailedPlanningApprovalDate.IsProvided())
               && (!IsQuestionActive(nameof(RequiredFurtherSteps)) || RequiredFurtherSteps.IsProvided())
               && (!IsQuestionActive(nameof(ApplicationForDetailedPlanningSubmittedDate)) || ApplicationForDetailedPlanningSubmittedDate.IsProvided())
               && (!IsQuestionActive(nameof(ExpectedPlanningApprovalDate)) || ExpectedPlanningApprovalDate.IsProvided())
               && (!IsQuestionActive(nameof(OutlinePlanningApprovalDate)) || OutlinePlanningApprovalDate.IsProvided())
               && (!IsQuestionActive(nameof(IsGrantFundingForAllHomesCoveredByApplication)) || IsGrantFundingForAllHomesCoveredByApplication.IsProvided())
               && (!IsQuestionActive(nameof(PlanningSubmissionDate)) || PlanningSubmissionDate.IsProvided())
               && (!IsQuestionActive(nameof(LandRegistryDetails)) || (LandRegistryDetails.IsProvided() && LandRegistryDetails!.IsAnswered()));
    }

    public bool IsQuestionActive(string fieldName)
    {
        return ActiveFields.Contains(fieldName);
    }

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
