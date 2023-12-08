namespace HE.Investment.AHP.Domain.HomeTypes;

// States should be ordered the same as application screens. So state later in flow should have greater value.
public enum HomeTypesWorkflowState
{
    Index = 1,
    List,
    RemoveHomeType,
    FinishHomeTypes,
    NewHomeTypeDetails,
    HomeTypeDetails,
    HomesForDisabledPeople,
    DisabledPeopleClientGroup,
    HomesForOlderPeople,
    HappiDesignPrinciples,
    DesignPlans,
    SupportedHousingInformation,
    RevenueFunding,
    MoveOnArrangements,
    ExitPlan,
    TypologyLocationAndDesign,
    HomeInformation,
    MoveOnAccommodation,
    PeopleGroupForSpecificDesignFeatures,
    BuildingInformation,
    BuildingInformationIneligible,
    CustomBuildProperty,
    TypeOfFacilities,
    AccessibilityStandards,
    AccessibilityCategory,
    CheckAnswers,
}
