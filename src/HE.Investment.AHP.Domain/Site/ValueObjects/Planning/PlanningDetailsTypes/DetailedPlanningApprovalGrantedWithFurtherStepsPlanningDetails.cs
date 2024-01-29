using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails : PlanningDetails, IQuestion
{
    public DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            detailedPlanningApprovalDate,
            requiredFurtherSteps: requiredFurtherSteps,
            isGrantFundingForAllHomes: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps;

    public override bool IsAnswered()
    {
        return ReferenceNumber.IsProvided() &&
               DetailedPlanningApprovalDate.IsProvided() &&
               RequiredFurtherSteps.IsProvided() &&
               IsGrantFundingForAllHomes.IsProvided();
    }
}
