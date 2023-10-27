using HE.Investment.AHP.Contract.HomeTypes;
using HE.InvestmentLoans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.HomeTypes;

public class HomeTypesWorkflow : IStateRouting<HomeTypesWorkflowState>
{
    private readonly HomeType _homeTypeModel;

    private readonly StateMachine<HomeTypesWorkflowState, Trigger> _machine;

    public HomeTypesWorkflow(HomeTypesWorkflowState currentHomeTypesWorkflowState, HomeType homeTypeModel)
    {
        _homeTypeModel = homeTypeModel;
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(currentHomeTypesWorkflowState);
        ConfigureTransitions();
    }

    public HomeTypesWorkflow()
        : this(HomeTypesWorkflowState.HousingType, new HomeType())
    {
    }

    public async Task<HomeTypesWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(HomeTypesWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(HomeTypesWorkflowState.Index)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HousingType);

        _machine.Configure(HomeTypesWorkflowState.HousingType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, () => _homeTypeModel.HousingTypeSection?.HousingType is HousingType.Undefined or HousingType.General)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.DisabledPeopleHousingType, () => _homeTypeModel.HousingTypeSection?.HousingType is HousingType.HousingForDisabledAndVulnerablePeople)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.OlderPeopleHousingType, () => _homeTypeModel.HousingTypeSection?.HousingType is HousingType.HousingForOlderPeople)
            .Permit(Trigger.Back, HomeTypesWorkflowState.Index);

        _machine.Configure(HomeTypesWorkflowState.HomeInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HousingType);

        _machine.Configure(HomeTypesWorkflowState.DisabledPeopleHousingType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HousingType);

        _machine.Configure(HomeTypesWorkflowState.OlderPeopleHousingType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HousingType);
    }
}
