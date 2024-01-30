using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApprovalGrantedPlanningDetails : PlanningDetails, IQuestion
{
    public DetailedPlanningApprovalGrantedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            detailedPlanningApprovalDate,
            isGrantFundingForAllHomes: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApprovalGranted;

    public override bool IsAnswered()
    {
        return ReferenceNumber.IsProvided() &&
               DetailedPlanningApprovalDate.IsProvided() &&
               IsGrantFundingForAllHomes.IsProvided();
    }
}
