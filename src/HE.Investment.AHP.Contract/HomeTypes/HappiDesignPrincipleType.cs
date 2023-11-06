using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes;

public enum HappiDesignPrincipleType
{
    Undefined = 0,

    [Description("Adaptability and ‘care ready’ design")]
    AdaptabilityAndCareReadyDesign,

    [Description("Balconies and outdoor space")]
    BalconiesAndOutdoorSpace,

    [Description("Daylight in the home and in shared spaces")]
    DaylightInTheHomeAndInSharedSpaces,

    [Description("Energy efficiency and sustainable design")]
    EnergyEfficiencyAndSustainableDesign,

    [Description("External shared surfaced and ‘home zones’")]
    ExternalSharedSurfacedAndHomeZones,

    [Description("Plants, trees and the natural environment")]
    PlantsTreesAndTheNaturalEnvironment,

    [Description("Positive use of circulation space")]
    PositiveUseOfCirculationSpace,

    [Description("Shared facilities and ‘hubs’")]
    SharedFacilitiesAndHubs,

    [Description("Space and flexibility")]
    SpaceAndFlexibility,

    [Description("Storage for belongings and bicycles")]
    StorageForBelongingsAndBicycles,

    [Description("None of these")]
    NoneOfThese,
}
