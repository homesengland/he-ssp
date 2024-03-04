using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.WWW.Workflows;

public class ProjectWorkflow : EncodedStateRouting<ProjectWorkflowState>
{
    private readonly ProjectDetails _model;

    public ProjectWorkflow(ProjectWorkflowState currentWorkflowState, ProjectDetails model, bool isLocked = false)
        : base(currentWorkflowState, isLocked)
    {
        _model = model;
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(ProjectWorkflowState state, bool? isReadOnlyMode = null)
    {
        var isStateEligible = BuildDeadEndConditions(state).All(isValid => isValid());
        return isStateEligible && state switch
        {
            ProjectWorkflowState.EnglandHousingDelivery => true,
            ProjectWorkflowState.NotEligibleForAnything => !IsEligibleForAnything(),
            ProjectWorkflowState.Name => true,
            ProjectWorkflowState.SupportRequiredActivities => true,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(ProjectWorkflowState.EnglandHousingDelivery)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.Name, IsEligibleForAnything)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.NotEligibleForAnything, () => !IsEligibleForAnything());

        Machine.Configure(ProjectWorkflowState.NotEligibleForAnything)
            .Permit(Trigger.Back, ProjectWorkflowState.EnglandHousingDelivery);

        Machine.Configure(ProjectWorkflowState.Name)
            .Permit(Trigger.Continue, ProjectWorkflowState.SupportRequiredActivities)
            .Permit(Trigger.Back, ProjectWorkflowState.EnglandHousingDelivery);

        Machine.Configure(ProjectWorkflowState.SupportRequiredActivities)
            .Permit(Trigger.Back, ProjectWorkflowState.Name);
    }

    private IEnumerable<Func<bool>> BuildDeadEndConditions(ProjectWorkflowState state)
    {
        if (state > ProjectWorkflowState.NotEligibleForAnything)
        {
            yield return IsEligibleForAnything;
        }
    }

    private bool IsEligibleForAnything() => _model.IsEnglandHousingDelivery;
}
