using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class HomeTypesWorkflow : IStateRouting<HomeTypesWorkflowState>
{
    private readonly HomeType? _homeTypeModel;

    private readonly StateMachine<HomeTypesWorkflowState, Trigger> _machine;

    private readonly bool _isLocked;

    private readonly bool _isReadOnly;

    public HomeTypesWorkflow(HomeTypesWorkflowState currentHomeTypesWorkflowState, HomeType? homeTypeModel, bool isReadOnly, bool isLocked = false)
    {
        _homeTypeModel = homeTypeModel;
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(currentHomeTypesWorkflowState);
        _isLocked = isLocked;
        _isReadOnly = isReadOnly;
        ConfigureTransitions();
    }

    public HomeTypesWorkflow(FullHomeType homeType, bool isReadOnly)
    {
        _isReadOnly = isReadOnly;
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
                homeType.ModernMethodsConstruction.ModernMethodsConstructionApplied,
                homeType.ModernMethodsConstruction.ModernMethodsConstructionCategories));
        _machine = new StateMachine<HomeTypesWorkflowState, Trigger>(HomeTypesWorkflowState.Index);
        ConfigureTransitions();
    }

    public HomeTypesWorkflow(bool isReadOnly)
        : this(HomeTypesWorkflowState.Index, null, isReadOnly)
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

    public bool CanBeAccessed(HomeTypesWorkflowState state, bool? isReadOnlyMode = null)
    {
        if (isReadOnlyMode ?? _isReadOnly)
        {
            return state switch
            {
                HomeTypesWorkflowState.Index => true,
                HomeTypesWorkflowState.List => true,
                HomeTypesWorkflowState.HomeTypeDetails => true,
                HomeTypesWorkflowState.CheckAnswers => true,
                _ => false,
            };
        }

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
            HomeTypesWorkflowState.RentToBuy => IsTenure(Tenure.RentToBuy),
            HomeTypesWorkflowState.RentToBuyIneligible => IsTenure(Tenure.RentToBuy) && !IsRentToBuyEligible(),
            HomeTypesWorkflowState.HomeOwnershipDisabilities => IsTenure(Tenure.HomeOwnershipLongTermDisabilities),
            HomeTypesWorkflowState.OlderPersonsSharedOwnership => IsTenure(Tenure.OlderPersonsSharedOwnership),
            HomeTypesWorkflowState.ProspectiveRentIneligible => (IsTenure(Tenure.SharedOwnership) && !IsSharedOwnershipEligible())
                                                                || (IsTenure(Tenure.HomeOwnershipLongTermDisabilities) && !IsHomeOwnershipEligible())
                                                                || (IsTenure(Tenure.OlderPersonsSharedOwnership) && !IsOlderPersonsSharedOwnershipEligible()),
            HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership => IsTenure(Tenure.AffordableRent, Tenure.SocialRent),
            HomeTypesWorkflowState.ExemptionJustification => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && IsExemptFromTheRightToSharedOwnership(),
            HomeTypesWorkflowState.ModernMethodsConstruction => true,
            HomeTypesWorkflowState.ModernMethodsConstructionCategories => !IsNotModernMethodsConstruction(),
            HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories => IsModernMethodsConstructionCategory1(),
            HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories => IsModernMethodsConstructionCategory2(),
            HomeTypesWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    public EncodedWorkflow<HomeTypesWorkflowState> GetEncodedWorkflow()
    {
        return new EncodedWorkflow<HomeTypesWorkflowState>(x => CanBeAccessed(x));
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
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.AffordableRent,
                () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SocialRent, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.SocialRent))
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.SharedOwnership,
                () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuy, () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.RentToBuy))
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.HomeOwnershipDisabilities,
                () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.OlderPersonsSharedOwnership,
                () => !IsNotMeetNationallyDescribedSpaceStandards() && IsTenure(Tenure.OlderPersonsSharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AccessibilityStandards, () => !IsAccessibleStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AccessibilityCategory, IsAccessibleStandards);

        _machine.Configure(HomeTypesWorkflowState.FloorAreaStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuy, () => IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => IsTenure(Tenure.OlderPersonsSharedOwnership))
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
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsSharedOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, IsSharedOwnershipEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.RentToBuy)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuyIneligible, () => !IsRentToBuyEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, IsRentToBuyEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.RentToBuyIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.RentToBuy);

        _machine.Configure(HomeTypesWorkflowState.HomeOwnershipDisabilities)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsHomeOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, IsHomeOwnershipEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.OlderPersonsSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsOlderPersonsSharedOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, IsOlderPersonsSharedOwnershipEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        _machine.Configure(HomeTypesWorkflowState.ProspectiveRentIneligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => IsTenure(Tenure.OlderPersonsSharedOwnership));

        _machine.Configure(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExemptionJustification, IsExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => !IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent));

        _machine.Configure(HomeTypesWorkflowState.ExemptionJustification)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction)
            .Permit(Trigger.Back, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        _machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, IsNotModernMethodsConstruction)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstructionCategories, () => !IsNotModernMethodsConstruction())
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership,
                () => !_isReadOnly && IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && !IsExemptFromTheRightToSharedOwnership())
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ExemptionJustification,
                () => !_isReadOnly && IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SharedOwnership, () => !_isReadOnly && IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RentToBuy, () => !_isReadOnly && IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => !_isReadOnly && IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => !_isReadOnly && IsTenure(Tenure.OlderPersonsSharedOwnership));

        _machine.Configure(HomeTypesWorkflowState.ModernMethodsConstructionCategories)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories, IsModernMethodsConstructionCategory1)
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories,
                () => !IsModernMethodsConstructionCategory1() && IsModernMethodsConstructionCategory2())
            .PermitIf(
                Trigger.Continue,
                HomeTypesWorkflowState.CheckAnswers,
                () => !IsModernMethodsConstructionCategory1() && !IsModernMethodsConstructionCategory2())
            .Permit(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction);

        _machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories, IsModernMethodsConstructionCategory2)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsModernMethodsConstructionCategory2())
            .Permit(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstructionCategories);

        _machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstructionCategories, () => !IsModernMethodsConstructionCategory1())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories, IsModernMethodsConstructionCategory1);

        _machine.Configure(HomeTypesWorkflowState.CheckAnswers)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.List, () => _isReadOnly)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction, () => !_isReadOnly && IsNotModernMethodsConstruction())
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ModernMethodsConstructionCategories,
                () => !_isReadOnly && IsModernMethodsConstructionOtherCategoryThan1Or2())
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories,
                () => !_isReadOnly && IsModernMethodsConstructionCategory1() && !IsModernMethodsConstructionCategory2())
            .PermitIf(
                Trigger.Back,
                HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories,
                () => !_isReadOnly && IsModernMethodsConstructionCategory2());
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

        if (state > HomeTypesWorkflowState.ProspectiveRentIneligible)
        {
            yield return IsSharedOwnershipEligible;
            yield return IsHomeOwnershipEligible;
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

    private bool IsSharedOwnershipEligible() => !(IsTenure(Tenure.SharedOwnership) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsRentToBuyEligible() => !(IsTenure(Tenure.RentToBuy) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsHomeOwnershipEligible() =>
        !(IsTenure(Tenure.HomeOwnershipLongTermDisabilities) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsOlderPersonsSharedOwnershipEligible() =>
        !(IsTenure(Tenure.OlderPersonsSharedOwnership) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsNotModernMethodsConstruction() => _homeTypeModel is { Conditionals.ModernMethodsConstructionApplied: YesNoType.No };

    private bool IsModernMethodsConstructionCategory1()
    {
        if (_homeTypeModel.IsProvided() && !IsNotModernMethodsConstruction())
        {
            return _homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType
                .Category1PreManufacturing3DPrimaryStructuralSystems);
        }

        return false;
    }

    private bool IsModernMethodsConstructionCategory2()
    {
        if (_homeTypeModel.IsProvided() && !IsNotModernMethodsConstruction())
        {
            return _homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType
                .Category2PreManufacturing2DPrimaryStructuralSystems);
        }

        return false;
    }

    private bool IsModernMethodsConstructionOtherCategoryThan1Or2()
    {
        if (_homeTypeModel.IsProvided() && !IsNotModernMethodsConstruction())
        {
            return !_homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType
                .Category1PreManufacturing3DPrimaryStructuralSystems) && !_homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType
                .Category2PreManufacturing2DPrimaryStructuralSystems);
        }

        return false;
    }
}
