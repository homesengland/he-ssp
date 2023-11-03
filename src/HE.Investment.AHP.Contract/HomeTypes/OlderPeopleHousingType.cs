using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes;

public enum OlderPeopleHousingType
{
    Undefined = 0,

    [Description("Designated supported homes for older people")]
    DesignatedSupportedHomes,

    [Description("Homes for older people with access to support as required")]
    HomesWithAccessToSupport,

    [Description("Homes for older people with some special design features")]
    HomesWithSomeSpecialDesignFeatures,

    [Description("Homes for older people with all special design features")]
    HomesWithAllSpecialDesignFeatures,
}
