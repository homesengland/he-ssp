using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Loans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.HomeTypes;

public class HomeTypesWorkflow : IStateRouting<HomeTypesWorkflowState>
{
    private readonly HomeType? _homeTypeModel;

    private readonly StateMachine<HomeTypesWorkflowState, Trigger> _machine;

    public HomeTypesWorkflow(HomeTypesWorkflowState currentHomeTypesWorkflowState, HomeType? homeTypeModel)
    {
        _homeTypeModel = homeTypeModel;
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(currentHomeTypesWorkflowState);
        ConfigureTransitions();
    }

    public HomeTypesWorkflow()
        : this(HomeTypesWorkflowState.Index, null)
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

        _machine.Configure(HomeTypesWorkflowState.FinishHomeTypes)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.List)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.FinishHomeTypes)
            .Permit(Trigger.Back, HomeTypesWorkflowState.Index);

        _machine.Configure(HomeTypesWorkflowState.RemoveHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.NewHomeTypeDetails)
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.HomeInformation,
                () => _homeTypeModel is { HousingType: HousingType.Undefined or HousingType.General })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.HomesForDisabledPeople,
                () => _homeTypeModel is { HousingType: HousingType.HomesForDisabledAndVulnerablePeople })
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, () => _homeTypeModel is { HousingType: HousingType.HomesForOlderPeople })
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeTypeDetails)
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.HomeInformation,
                () => _homeTypeModel is { HousingType: HousingType.Undefined or HousingType.General })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.HomesForDisabledPeople,
                () => _homeTypeModel is { HousingType: HousingType.HomesForDisabledAndVulnerablePeople })
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, () => _homeTypeModel is { HousingType: HousingType.HomesForOlderPeople })
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeInformation)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List)
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.HomeTypeDetails,
                () => _homeTypeModel is { HousingType: HousingType.Undefined or HousingType.General })
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.TypologyLocationAndDesign,
                () => _homeTypeModel is { HousingType: HousingType.HomesForDisabledAndVulnerablePeople or HousingType.HomesForOlderPeople });

        _machine.Configure(HomeTypesWorkflowState.HomesForDisabledPeople)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.DisabledPeopleClientGroup)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        _machine.Configure(HomeTypesWorkflowState.DisabledPeopleClientGroup)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomesForDisabledPeople);

        _machine.Configure(HomeTypesWorkflowState.HomesForOlderPeople)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        _machine.Configure(HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.DesignPlans)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomesForOlderPeople, () => _homeTypeModel is { HousingType: HousingType.HomesForOlderPeople })
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.DisabledPeopleClientGroup,
                () => _homeTypeModel is { HousingType: HousingType.HomesForDisabledAndVulnerablePeople });

        _machine.Configure(HomeTypesWorkflowState.DesignPlans)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.SupportedHousingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HappiDesignPrinciples);

        _machine.Configure(HomeTypesWorkflowState.SupportedHousingInformation)
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.ExitPlan,
                () => _homeTypeModel is { Conditionals.RevenueFundingType: RevenueFundingType.Undefined }
                    or { Conditionals.ShortStayAccommodation: YesNoType.Undefined })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.ExitPlan,
                () => _homeTypeModel is (
                { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededButNotIdentified }
                    or { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNotNeeded })
                    and { Conditionals.ShortStayAccommodation: YesNoType.No })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.MoveOnArrangements,
                () => _homeTypeModel is (
                { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededButNotIdentified }
                    or { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNotNeeded })
                    and { Conditionals.ShortStayAccommodation: YesNoType.Yes })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.RevenueFunding,
                () => _homeTypeModel is { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededAndIdentified }
                    and not { Conditionals.ShortStayAccommodation: YesNoType.Undefined })
            .Permit(Trigger.Back, HomeTypesWorkflowState.DesignPlans);

        _machine.Configure(HomeTypesWorkflowState.RevenueFunding)
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.ExitPlan,
                () => _homeTypeModel is not { Conditionals.ShortStayAccommodation: YesNoType.Yes })
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.MoveOnArrangements,
                () => _homeTypeModel is { Conditionals.ShortStayAccommodation: YesNoType.Yes })
            .Permit(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation);

        _machine.Configure(HomeTypesWorkflowState.MoveOnArrangements)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ExitPlan)
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.SupportedHousingInformation,
                () => _homeTypeModel is not { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededAndIdentified })
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.RevenueFunding,
                () => _homeTypeModel is { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededAndIdentified });

        _machine.Configure(HomeTypesWorkflowState.ExitPlan)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.TypologyLocationAndDesign)
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.SupportedHousingInformation,
                () => _homeTypeModel is not { Conditionals.ShortStayAccommodation: YesNoType.Yes }
                    or { Conditionals.RevenueFundingType: RevenueFundingType.Undefined })
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.MoveOnArrangements,
                () => _homeTypeModel is { Conditionals.ShortStayAccommodation: YesNoType.Yes }
                    and not { Conditionals.RevenueFundingType: RevenueFundingType.Undefined });

        _machine.Configure(HomeTypesWorkflowState.TypologyLocationAndDesign)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HomeInformation)
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ExitPlan,
                () => _homeTypeModel is not { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededAndIdentified })
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.RevenueFunding,
                () => _homeTypeModel is { Conditionals.RevenueFundingType: RevenueFundingType.RevenueFundingNeededAndIdentified });
    }
}
