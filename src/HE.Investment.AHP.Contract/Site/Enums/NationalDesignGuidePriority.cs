using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site.Enums;

public enum NationalDesignGuidePriority
{
    Undefined = 0,

    [Description("Built form")]
    BuiltForm,

    [Description("Context")]
    Context,

    [Description("Homes and buildings")]
    HomesAndBuildings,

    [Description("Identity")]
    Identity,

    [Description("Lifespan")]
    Lifespan,

    [Description("Movement")]
    Movement,

    [Description("Nature")]
    Nature,

    [Description("Public spaces")]
    PublicSpaces,

    [Description("Resources")]
    Resources,

    [Description("Uses")]
    Uses,

    [Description("None of the above")]
    NoneOfTheAbove,
}
