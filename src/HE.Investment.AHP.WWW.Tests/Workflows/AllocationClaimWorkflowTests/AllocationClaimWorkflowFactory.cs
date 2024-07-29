using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.AllocationClaimWorkflowTests;

internal static class AllocationClaimWorkflowFactory
{
    public static AllocationClaimWorkflow Create(
        MilestoneType milestoneType,
        bool canBeClaimed,
        AllocationClaimWorkflowState state = AllocationClaimWorkflowState.CheckAnswers)
    {
        var model = new MilestoneClaim(
            milestoneType,
            MilestoneStatus.Due,
            1000000m,
            0.3m,
            new DateDetails("01", "01", "2025"),
            null,
            null,
            canBeClaimed,
            null,
            null,
            true);

        return new AllocationClaimWorkflow(state, model);
    }
}
