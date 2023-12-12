using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Workflow;
using HE.Investments.Loans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.HomeTypes;

public class HomeTypesWorkflow : IStateRouting<HomeTypesWorkflowState>
{
    private readonly HomeType? _homeTypeModel;

    private readonly StateMachine<HomeTypesWorkflowState, Trigger> _machine;

    private readonly bool _isLocked;

    public HomeTypesWorkflow(HomeTypesWorkflowState currentHomeTypesWorkflowState, HomeType? homeTypeModel, bool isLocked = false)
    {
        _homeTypeModel = homeTypeModel;
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(currentHomeTypesWorkflowState);
        _isLocked = isLocked;
        ConfigureTransitions();
    }

    public HomeTypesWorkflow(FullHomeType homeType)
    {
        _homeTypeModel = new HomeType(
            homeType.Id,
            homeType.Name,
            homeType.HousingType,
            homeType.Tenure,
            new HomeTypeConditionals(
                homeType.SupportedHousing?.ShortStayAccommodation ?? YesNoType.Undefined,
                homeType.SupportedHousing?.RevenueFundingType ?? RevenueFundingType.Undefined,
                homeType.HomeInformation.BuildingType,
                homeType.HomeInformation.AccessibilityStandards,
                homeType.HomeInformation.MeetNationallyDescribedSpaceStandards,
                homeType.TenureDetails.ExemptFromTheRightToSharedOwnership,
                homeType.TenureDetails.IsProspectiveRentIneligible,
                homeType.TenureDetails.IsSharedOwnershipIneligible));
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(HomeTypesWorkflowState.Index);
        ConfigureTransitions();
    }

    public HomeTypesWorkflow()
        : this(HomeTypesWorkflowState.Index, null)
    {
    }

    public async Task<HomeTypesWorkflowState> NextState(Trigger trigger)
    {
        if (_isLocked)
        {
            return _machine.State;
        }

        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(HomeTypesWorkflowState nextState)
    {
        return Task.FromResult(CanBeAccessed(nextState));
    }

    public bool CanBeAccessed(HomeTypesWorkflowState state)
    {
        var isStateEligible = BuildDeadEndConditions(state).All(isValid => isValid());
        return isStateEligible && state switch
        {
            HomeTypesWorkflowState.Index => true,
            HomeTypesWorkflowState.List => true,
            HomeTypesWorkflowState.RemoveHomeType => true,
            HomeTypesWorkflowState.FinishHomeTypes => true,
            HomeTypesWorkflowState.NewHomeTypeDetails => true,
            HomeTypesWorkflowState.HomeTypeDetails => true,
            HomeTypesWorkflowState.HomeInformation => true,
            HomeTypesWorkflowState.HomesForDisabledPeople => IsDisabledHomeType(),
            HomeTypesWorkflowState.DisabledPeopleClientGroup => IsDisabledHomeType(),
            HomeTypesWorkflowState.HomesForOlderPeople => IsOlderHomeType(),
            HomeTypesWorkflowState.HappiDesignPrinciples => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.DesignPlans => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.SupportedHousingInformation => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.RevenueFunding => IsNotGeneralHomeType() && IsRevenueFundingIdentified(),
            HomeTypesWorkflowState.ExitPlan => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.MoveOnArrangements => IsNotGeneralHomeType() && IsShortStay(),
            HomeTypesWorkflowState.TypologyLocationAndDesign => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.MoveOnAccommodation => IsGeneralHomeType(),
            HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures => IsNotGeneralHomeType(),
            HomeTypesWorkflowState.BuildingInformation => true,
            HomeTypesWorkflowState.BuildingInformationIneligible => !IsBuildingInformationEligible(),
            HomeTypesWorkflowState.CustomBuildProperty => true,
            HomeTypesWorkflowState.TypeOfFacilities => true,
            HomeTypesWorkflowState.AccessibilityStandards => true,
            HomeTypesWorkflowState.AccessibilityCategory => IsAccessibleStandards(),
            HomeTypesWorkflowState.FloorArea => true,
            HomeTypesWorkflowState.FloorAreaStandards => IsNotMeetNationallyDescribedSpaceStandards(),
            HomeTypesWorkflowState.AffordableRent => IsTenure(Tenure.AffordableRent),
            HomeTypesWorkflowState.AffordableRentIneligible => IsTenure(Tenure.AffordableRent) && !IsAffordableRentEligible(),
            HomeTypesWorkflowState.SocialRent => IsTenure(Tenure.SocialRent),
            HomeTypesWorkflowState.SharedOwnership => IsTenure(Tenure.SharedOwnership),
            HomeTypesWorkflowState.SharedOwnershipIneligible => IsTenure(Tenure.SharedOwnership) && !IsSharedOwnershipEligible(),
            HomeTypesWorkflowState.RentToBuy => IsTenure(Tenure.RentToBuy),
            HomeTypesWorkflowState.RentToBuyIneligible => IsTenure(Tenure.RentToBuy) && !IsRentToBuyEligible(),
            HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership => IsTenure(Tenure.AffordableRent, Tenure.SocialRent),
            HomeTypesWorkflowState.ExemptionJustification => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && IsExemptFromTheRightToSharedOwnership(),
            HomeTypesWorkflowState.CheckAnswers => true,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    public EncodedWorkflow<HomeTypesWorkflowState> GetEncodedWorkflow()
    {
        return new EncodedWorkflow<HomeTypesWorkflowState>(CanBeAccessed);
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
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, IsDisabledHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeTypeDetails)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, IsDisabledHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        _machine.Configure(HomeTypesWorkflowState.HomeInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnAccommodation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures, IsNotGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails, IsGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.TypologyLocationAndDesign, IsNotGeneralHomeType);

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
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.DisabledPeopleClientGroup, IsDisabledHomeType);

        _machine.Configure(HomeTypesWorkflowState.DesignPlans)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.SupportedHousingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HappiDesignPrinciples);

        _machine.Configure(HomeTypesWorkflowState.SupportedHousingInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExitPlan, () => !IsRevenueFundingIdentified() && !IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnArrangements, () => !IsRevenueFundingIdentified() && IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RevenueFunding, IsRevenueFundingIdentified)
            .Permit(Trigger.Back, HomeTypesWorkflowState.DesignPlans);

        _machine.Configure(HomeTypesWorkflowState.RevenueFunding)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExitPlan, () => !IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnArrangements, IsShortStay)
            .Permit(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation);

        _machine.Configure(HomeTypesWorkflowState.MoveOnArrangements)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ExitPlan)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation, () => !IsRevenueFundingIdentified())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RevenueFunding, IsRevenueFundingIdentified);

        _machine.Configure(HomeTypesWorkflowState.ExitPlan)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.TypologyLocationAndDesign)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation, () => !IsRevenueFundingIdentified() && !IsShortStay())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.MoveOnArrangements, IsShortStay)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RevenueFunding, () => IsRevenueFundingIdentified() && !IsShortStay());

        _machine.Configure(HomeTypesWorkflowState.TypologyLocationAndDesign)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HomeInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.ExitPlan);

        _machine.Configure(HomeTypesWorkflowState.MoveOnAccommodation)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.BuildingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeInformation);

        _machine.Configure(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.BuildingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeInformation);

        _machine.Configure(HomeTypesWorkflowState.BuildingInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CustomBuildProperty, IsBuildingInformationEligible)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.BuildingInformationIneligible, () => !IsBuildingInformationEligible())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures, IsNotGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.MoveOnAccommodation, IsGeneralHomeType);

        _machine.Configure(HomeTypesWorkflowState.BuildingInformationIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.BuildingInformation);

        _machine.Configure(HomeTypesWorkflowState.CustomBuildProperty)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.TypeOfFacilities)
            .Permit(Trigger.Back, HomeTypesWorkflowState.BuildingInformation);

        _machine.Configure(HomeTypesWorkflowState.TypeOfFacilities)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.AccessibilityStandards)
            .Permit(Trigger.Back, HomeTypesWorkflowState.CustomBuildProperty);

        _machine.Configure(HomeTypesWorkflowState.AccessibilityStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.FloorArea, () => !IsAccessibleStandards())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AccessibilityCategory, IsAccessibleStandards)
            .Permit(Trigger.Back, HomeTypesWorkflowState.TypeOfFacilities);

        _machine.Configure(HomeTypesWorkflowState.AccessibilityCategory)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.FloorArea)
            .Permit(Trigger.Back, HomeTypesWorkflowState.AccessibilityStandards);

        _machine.Configure(HomeTypesWorkflowState.FloorArea)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRent, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SocialRent, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.SocialRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SharedOwnership, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuy, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.HomeOwnershipLongTermDisabilities, Tenure.OlderPersonsSharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AccessibilityStandards, () => !IsAccessibleStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AccessibilityCategory, IsAccessibleStandards);

        _machine.Configure(HomeTypesWorkflowState.FloorAreaStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuy, () => IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities, Tenure.OlderPersonsSharedOwnership))
            .Permit(Trigger.Back, HomeTypesWorkflowState.FloorArea);

        _machine.Configure(HomeTypesWorkflowState.AffordableRent)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRentIneligible, () => !IsAffordableRentEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership, IsAffordableRentEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.AffordableRentIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.AffordableRent);

        _machine.Configure(HomeTypesWorkflowState.SocialRent)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.SharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SharedOwnershipIneligible, () => !IsSharedOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, IsSharedOwnershipEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.SharedOwnershipIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.SharedOwnership);

        _machine.Configure(HomeTypesWorkflowState.RentToBuy)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuyIneligible, () => !IsRentToBuyEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, IsRentToBuyEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.RentToBuyIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.RentToBuy);

        _machine.Configure(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExemptionJustification, IsExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent));

        _machine.Configure(HomeTypesWorkflowState.ExemptionJustification)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        _machine.Configure(HomeTypesWorkflowState.CheckAnswers)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership, () => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && !IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ExemptionJustification, () => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RentToBuy, () => IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => IsTenure(Tenure.OlderPersonsSharedOwnership, Tenure.HomeOwnershipLongTermDisabilities) && !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, () => IsTenure(Tenure.OlderPersonsSharedOwnership, Tenure.HomeOwnershipLongTermDisabilities) && IsNotMeetNationallyDescribedSpaceStandards());
    }

    private IEnumerable<Func<bool>> BuildDeadEndConditions(HomeTypesWorkflowState state)
    {
        if (state > HomeTypesWorkflowState.BuildingInformationIneligible)
        {
            yield return IsBuildingInformationEligible;
        }

        if (state > HomeTypesWorkflowState.AffordableRentIneligible)
        {
            yield return IsAffordableRentEligible;
        }

        if (state > HomeTypesWorkflowState.SharedOwnershipIneligible)
        {
            yield return IsSharedOwnershipEligible;
        }

        if (state > HomeTypesWorkflowState.RentToBuyIneligible)
        {
            yield return IsRentToBuyEligible;
        }
    }

    private bool IsGeneralHomeType() => _homeTypeModel is { HousingType: HousingType.Undefined or HousingType.General };

    private bool IsNotGeneralHomeType() => !IsGeneralHomeType();

    private bool IsDisabledHomeType() => _homeTypeModel is { HousingType: HousingType.HomesForDisabledAndVulnerablePeople };

    private bool IsOlderHomeType() => _homeTypeModel is { HousingType: HousingType.HomesForOlderPeople };

    private bool IsRevenueFundingIdentified() => _homeTypeModel is
    {
        Conditionals.RevenueFundingType: RevenueFundingType.Undefined or RevenueFundingType.RevenueFundingNeededAndIdentified
    };

    private bool IsShortStay() => _homeTypeModel is { Conditionals.ShortStayAccommodation: YesNoType.Undefined or YesNoType.Yes };

    private bool IsBuildingInformationEligible() => !(IsGeneralHomeType() && _homeTypeModel is { Conditionals.BuildingType: BuildingType.Bedsit });

    private bool IsAccessibleStandards() => _homeTypeModel is { Conditionals.AccessibleStandards: YesNoType.Yes };

    private bool IsNotMeetNationallyDescribedSpaceStandards() => _homeTypeModel is { Conditionals.MeetNationallyDescribedSpaceStandards: YesNoType.No };

    private bool IsExemptFromTheRightToSharedOwnership() => _homeTypeModel is { Conditionals.ExemptFromTheRightToSharedOwnership: YesNoType.Yes };

    private bool IsTenure(params Tenure[] tenure)
    {
        return tenure.Contains(_homeTypeModel?.Tenure ?? Tenure.Undefined);
    }

    private bool IsAffordableRentEligible() => !(IsTenure(Tenure.AffordableRent) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsSharedOwnershipEligible() => !(IsTenure(Tenure.SharedOwnership) && _homeTypeModel is { Conditionals.IsSharedOwnershipIneligible: true });

    private bool IsRentToBuyEligible() => !(IsTenure(Tenure.RentToBuy) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });
}
