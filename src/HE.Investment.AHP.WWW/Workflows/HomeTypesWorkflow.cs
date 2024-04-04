using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class HomeTypesWorkflow : EncodedStateRouting<HomeTypesWorkflowState>
{
    private readonly HomeType? _homeTypeModel;

    private readonly bool _isReadOnly;

    public HomeTypesWorkflow(HomeTypesWorkflowState currentHomeTypesWorkflowState, HomeType? homeTypeModel, bool isReadOnly, bool isLocked = false)
        : base(currentHomeTypesWorkflowState, isLocked)
    {
        _homeTypeModel = homeTypeModel;
        _isReadOnly = isReadOnly;
        ConfigureTransitions();
    }

    public HomeTypesWorkflow(FullHomeType homeType)
        : base(HomeTypesWorkflowState.Index, false)
    {
        _isReadOnly = homeType.Application.IsReadOnly;
        _homeTypeModel = new HomeType(
            homeType.Application,
            homeType.Id,
            homeType.Name,
            homeType.HousingType,
            new HomeTypeConditionals(
                homeType.SupportedHousing?.ShortStayAccommodation ?? YesNoType.Undefined,
                homeType.SupportedHousing?.RevenueFundingType ?? RevenueFundingType.Undefined,
                homeType.HomeInformation.BuildingType,
                homeType.HomeInformation.AccessibilityStandards,
                homeType.HomeInformation.MeetNationallyDescribedSpaceStandards,
                homeType.TenureDetails.ExemptFromTheRightToSharedOwnership,
                homeType.TenureDetails.IsProspectiveRentIneligible,
                homeType.ModernMethodsConstruction.SiteUsingModernMethodsOfConstruction,
                homeType.ModernMethodsConstruction.ModernMethodsConstructionApplied,
                homeType.ModernMethodsConstruction.ModernMethodsConstructionCategories));
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(HomeTypesWorkflowState state, bool? isReadOnlyMode = null)
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
            HomeTypesWorkflowState.ModernMethodsConstruction => IsMmcRequired(),
            HomeTypesWorkflowState.ModernMethodsConstructionCategories => IsMmcUsed(),
            HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories => IsMmc3DCategoryUsed(),
            HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories => IsMmc2DCategoryUsed(),
            HomeTypesWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(HomeTypesWorkflowState.Index)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List);

        Machine.Configure(HomeTypesWorkflowState.FinishHomeTypes)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        Machine.Configure(HomeTypesWorkflowState.List)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.FinishHomeTypes)
            .Permit(Trigger.Back, HomeTypesWorkflowState.Index);

        Machine.Configure(HomeTypesWorkflowState.RemoveHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        Machine.Configure(HomeTypesWorkflowState.NewHomeTypeDetails)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, IsDisabledHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        Machine.Configure(HomeTypesWorkflowState.HomeTypeDetails)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeInformation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForDisabledPeople, IsDisabledHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .Permit(Trigger.Back, HomeTypesWorkflowState.List);

        Machine.Configure(HomeTypesWorkflowState.HomeInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnAccommodation, IsGeneralHomeType)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures, IsNotGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails, IsGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.TypologyLocationAndDesign, IsNotGeneralHomeType);

        Machine.Configure(HomeTypesWorkflowState.HomesForDisabledPeople)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.DisabledPeopleClientGroup)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        Machine.Configure(HomeTypesWorkflowState.DisabledPeopleClientGroup)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomesForDisabledPeople);

        Machine.Configure(HomeTypesWorkflowState.HomesForOlderPeople)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeTypeDetails);

        Machine.Configure(HomeTypesWorkflowState.HappiDesignPrinciples)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.DesignPlans)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomesForOlderPeople, IsOlderHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.DisabledPeopleClientGroup, IsDisabledHomeType);

        Machine.Configure(HomeTypesWorkflowState.DesignPlans)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.SupportedHousingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HappiDesignPrinciples);

        Machine.Configure(HomeTypesWorkflowState.SupportedHousingInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExitPlan, () => !IsRevenueFundingIdentified() && !IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnArrangements, () => !IsRevenueFundingIdentified() && IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RevenueFunding, IsRevenueFundingIdentified)
            .Permit(Trigger.Back, HomeTypesWorkflowState.DesignPlans);

        Machine.Configure(HomeTypesWorkflowState.RevenueFunding)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExitPlan, () => !IsShortStay())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.MoveOnArrangements, IsShortStay)
            .Permit(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation);

        Machine.Configure(HomeTypesWorkflowState.MoveOnArrangements)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ExitPlan)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation, () => !IsRevenueFundingIdentified())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RevenueFunding, IsRevenueFundingIdentified);

        Machine.Configure(HomeTypesWorkflowState.ExitPlan)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.TypologyLocationAndDesign)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SupportedHousingInformation, () => !IsRevenueFundingIdentified() && !IsShortStay())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.MoveOnArrangements, IsShortStay)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RevenueFunding, () => IsRevenueFundingIdentified() && !IsShortStay());

        Machine.Configure(HomeTypesWorkflowState.TypologyLocationAndDesign)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.HomeInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.ExitPlan);

        Machine.Configure(HomeTypesWorkflowState.MoveOnAccommodation)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.BuildingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeInformation);

        Machine.Configure(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.BuildingInformation)
            .Permit(Trigger.Back, HomeTypesWorkflowState.HomeInformation);

        Machine.Configure(HomeTypesWorkflowState.BuildingInformation)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CustomBuildProperty, IsBuildingInformationEligible)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.BuildingInformationIneligible, () => !IsBuildingInformationEligible())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures, IsNotGeneralHomeType)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.MoveOnAccommodation, IsGeneralHomeType);

        Machine.Configure(HomeTypesWorkflowState.BuildingInformationIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.BuildingInformation);

        Machine.Configure(HomeTypesWorkflowState.CustomBuildProperty)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.TypeOfFacilities)
            .Permit(Trigger.Back, HomeTypesWorkflowState.BuildingInformation);

        Machine.Configure(HomeTypesWorkflowState.TypeOfFacilities)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.AccessibilityStandards)
            .Permit(Trigger.Back, HomeTypesWorkflowState.CustomBuildProperty);

        Machine.Configure(HomeTypesWorkflowState.AccessibilityStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.FloorArea, () => !IsAccessibleStandards())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AccessibilityCategory, IsAccessibleStandards)
            .Permit(Trigger.Back, HomeTypesWorkflowState.TypeOfFacilities);

        Machine.Configure(HomeTypesWorkflowState.AccessibilityCategory)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.FloorArea)
            .Permit(Trigger.Back, HomeTypesWorkflowState.AccessibilityStandards);

        Machine.Configure(HomeTypesWorkflowState.FloorArea)
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

        Machine.Configure(HomeTypesWorkflowState.FloorAreaStandards)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuy, () => IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => IsTenure(Tenure.OlderPersonsSharedOwnership))
            .Permit(Trigger.Back, HomeTypesWorkflowState.FloorArea);

        Machine.Configure(HomeTypesWorkflowState.AffordableRent)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.AffordableRentIneligible, () => !IsAffordableRentEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership, IsAffordableRentEligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.AffordableRentIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.AffordableRent);

        Machine.Configure(HomeTypesWorkflowState.SocialRent)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.SharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsSharedOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => IsSharedOwnershipEligible() && IsMmcRequired())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => IsSharedOwnershipEligible() && !IsMmcRequired())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.RentToBuy)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.RentToBuyIneligible, () => !IsRentToBuyEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => IsRentToBuyEligible() && IsMmcRequired())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => IsRentToBuyEligible() && !IsMmcRequired())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.RentToBuyIneligible)
            .Permit(Trigger.Back, HomeTypesWorkflowState.RentToBuy);

        Machine.Configure(HomeTypesWorkflowState.HomeOwnershipDisabilities)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsHomeOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => IsHomeOwnershipEligible() && IsMmcRequired())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => IsHomeOwnershipEligible() && !IsMmcRequired())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.OlderPersonsSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ProspectiveRentIneligible, () => !IsOlderPersonsSharedOwnershipEligible())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => IsOlderPersonsSharedOwnershipEligible() && IsMmcRequired())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => IsOlderPersonsSharedOwnershipEligible() && !IsMmcRequired())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorArea, () => !IsNotMeetNationallyDescribedSpaceStandards())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.FloorAreaStandards, IsNotMeetNationallyDescribedSpaceStandards);

        Machine.Configure(HomeTypesWorkflowState.ProspectiveRentIneligible)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => IsTenure(Tenure.OlderPersonsSharedOwnership));

        Machine.Configure(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ExemptionJustification, IsExemptFromTheRightToSharedOwnership)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, () => !IsExemptFromTheRightToSharedOwnership() && IsMmcRequired())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsExemptFromTheRightToSharedOwnership() && !IsMmcRequired())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.AffordableRent, () => IsTenure(Tenure.AffordableRent))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SocialRent, () => IsTenure(Tenure.SocialRent));

        Machine.Configure(HomeTypesWorkflowState.ExemptionJustification)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction, IsMmcRequired)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsMmcRequired())
            .Permit(Trigger.Back, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        Machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsMmcUsed())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstructionCategories, IsMmcUsed)
            .PermitSection(BackToTenure, () => true);

        Machine.Configure(HomeTypesWorkflowState.ModernMethodsConstructionCategories)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories, IsMmc3DCategoryUsed)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories, () => !IsMmc3DCategoryUsed() && IsMmc2DCategoryUsed())
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsMmc3DCategoryUsed() && !IsMmc2DCategoryUsed())
            .Permit(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction);

        Machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories, IsMmc2DCategoryUsed)
            .PermitIf(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers, () => !IsMmc2DCategoryUsed())
            .Permit(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstructionCategories);

        Machine.Configure(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstructionCategories, () => !IsMmc3DCategoryUsed())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories, IsMmc3DCategoryUsed);

        Machine.Configure(HomeTypesWorkflowState.CheckAnswers)
            .Permit(Trigger.Continue, HomeTypesWorkflowState.List)
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.List, () => _isReadOnly)
            .PermitSection(BackToMmc, () => !_isReadOnly && IsMmcRequired())
            .PermitSection(BackToTenure, () => !_isReadOnly && !IsMmcRequired());
    }

    private void BackToMmc(StateMachine<HomeTypesWorkflowState, Trigger>.StateConfiguration config)
    {
        config.PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction, () => !IsMmcUsed())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstructionCategories, () => IsMmcUsed() && !IsMmc3DCategoryUsed() && !IsMmc2DCategoryUsed())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories, () => IsMmcUsed() && IsMmc3DCategoryUsed() && !IsMmc2DCategoryUsed())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories, () => IsMmcUsed() && IsMmc2DCategoryUsed());
    }

    private void BackToTenure(StateMachine<HomeTypesWorkflowState, Trigger>.StateConfiguration config)
    {
        config.PermitIf(Trigger.Back, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership, () => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && !IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.ExemptionJustification, () => IsTenure(Tenure.AffordableRent, Tenure.SocialRent) && IsExemptFromTheRightToSharedOwnership())
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.SharedOwnership, () => IsTenure(Tenure.SharedOwnership))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.RentToBuy, () => IsTenure(Tenure.RentToBuy))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.HomeOwnershipDisabilities, () => IsTenure(Tenure.HomeOwnershipLongTermDisabilities))
            .PermitIf(Trigger.Back, HomeTypesWorkflowState.OlderPersonsSharedOwnership, () => IsTenure(Tenure.OlderPersonsSharedOwnership));
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
        return tenure.Contains(_homeTypeModel?.Application.Tenure ?? Tenure.Undefined);
    }

    private bool IsAffordableRentEligible() => !(IsTenure(Tenure.AffordableRent) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsSharedOwnershipEligible() => !(IsTenure(Tenure.SharedOwnership) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsRentToBuyEligible() => !(IsTenure(Tenure.RentToBuy) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsHomeOwnershipEligible() =>
        !(IsTenure(Tenure.HomeOwnershipLongTermDisabilities) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsOlderPersonsSharedOwnershipEligible() =>
        !(IsTenure(Tenure.OlderPersonsSharedOwnership) && _homeTypeModel is { Conditionals.IsProspectiveRentIneligible: true });

    private bool IsMmcRequired() => _homeTypeModel is { Conditionals.SiteUsingModernMethodsOfConstruction: SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes };

    private bool IsMmcUsed() => IsMmcRequired() && _homeTypeModel is { Conditionals.ModernMethodsConstructionApplied: YesNoType.Yes };

    private bool IsMmc3DCategoryUsed() => IsMmcUsed() && _homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(
        ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems);

    private bool IsMmc2DCategoryUsed() => IsMmcUsed() && _homeTypeModel!.Conditionals.ModernMethodsConstructionCategories.Contains(
        ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems);
}
