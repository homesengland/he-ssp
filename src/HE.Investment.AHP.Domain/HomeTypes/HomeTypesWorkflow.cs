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
        : this(HomeTypesWorkflowState.NewHomeTypeDetails, new HomeType())
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
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.List)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.NewHomeTypeDetails)
            .Permit(Trigger.Back, HomeTypesWorkflowState.Index);

        _machine.Configure(HomeTypesWorkflowState.RemoveHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.NewHomeTypeDetails)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, () => _homeTypeModel.HousingType is HousingType.Undefined or HousingType.General)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, () => _homeTypeModel.HousingType is HousingType.HomesForDisabledAndVulnerablePeople)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, () => _homeTypeModel.HousingType is HousingType.HomesForOlderPeople)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeTypeDetails)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, () => _homeTypeModel.HousingType is HousingType.Undefined or HousingType.General)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, () => _homeTypeModel.HousingType is HousingType.HomesForDisabledAndVulnerablePeople)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, () => _homeTypeModel.HousingType is HousingType.HomesForOlderPeople)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        _machine.Configure(HomeTypesWorkflowState.HomesForDisabledPeople)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        _machine.Configure(HomeTypesWorkflowState.HomesForOlderPeople)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);
    }
}
