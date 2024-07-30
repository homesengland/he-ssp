using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Workflows;

public class AllocationClaimWorkflow : EncodedStateRouting<AllocationClaimWorkflowState>
{
    private readonly MilestoneClaim _model;

    public AllocationClaimWorkflow(AllocationClaimWorkflowState currentWorkflowState, MilestoneClaim model, bool isLocked = false)
        : base(currentWorkflowState, isLocked)
    {
        _model = model;
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(AllocationClaimWorkflowState state, bool? isReadOnlyMode = null)
    {
        if (isReadOnlyMode ?? false)
        {
            return state == AllocationClaimWorkflowState.CheckAnswers;
        }

        return state switch
        {
            AllocationClaimWorkflowState.CostsIncurred => _model is { Type: MilestoneType.Acquisition, CanBeClaimed: true },
            AllocationClaimWorkflowState.AchievementDate => _model.CanBeClaimed,
            AllocationClaimWorkflowState.Confirmation => _model.CanBeClaimed,
            AllocationClaimWorkflowState.CheckAnswers => _model.CanBeClaimed,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(AllocationClaimWorkflowState.CostsIncurred)
            .Permit(Trigger.Continue, AllocationClaimWorkflowState.AchievementDate);

        Machine.Configure(AllocationClaimWorkflowState.AchievementDate)
            .Permit(Trigger.Continue, AllocationClaimWorkflowState.Confirmation)
            .PermitIf(Trigger.Back, AllocationClaimWorkflowState.CostsIncurred, () => _model.Type == MilestoneType.Acquisition);

        Machine.Configure(AllocationClaimWorkflowState.Confirmation)
            .Permit(Trigger.Continue, AllocationClaimWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, AllocationClaimWorkflowState.AchievementDate);

        Machine.Configure(AllocationClaimWorkflowState.CheckAnswers)
            .Permit(Trigger.Continue, AllocationClaimWorkflowState.Overview)
            .Permit(Trigger.Back, AllocationClaimWorkflowState.Confirmation);
    }
}
