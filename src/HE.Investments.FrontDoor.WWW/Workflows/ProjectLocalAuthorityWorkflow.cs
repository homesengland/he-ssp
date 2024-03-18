using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.LocalAuthority;
using HE.Investments.FrontDoor.Contract.Site;
using Stateless;

namespace HE.Investments.FrontDoor.WWW.Workflows;

public class ProjectLocalAuthorityWorkflow : IStateRouting<LocalAuthorityWorkflowState>
{
    private readonly StateMachine<LocalAuthorityWorkflowState, Trigger> _machine;

    public ProjectLocalAuthorityWorkflow(LocalAuthorityWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<LocalAuthorityWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<LocalAuthorityWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(LocalAuthorityWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(LocalAuthorityWorkflowState.Search)
            .Permit(Trigger.Continue, LocalAuthorityWorkflowState.SearchResult);

        _machine.Configure(LocalAuthorityWorkflowState.SearchResult)
            .Permit(Trigger.Back, LocalAuthorityWorkflowState.Search);

        _machine.Configure(LocalAuthorityWorkflowState.NotFound)
            .Permit(Trigger.Back, LocalAuthorityWorkflowState.Search);
    }
}
