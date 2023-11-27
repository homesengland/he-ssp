using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum PeopleGroupForSpecificDesignFeaturesType
{
    Undefined = 0,

    [Description("People from ethnic minority backgrounds")]
    EthnicMinority,

    [Description("Disabled people")]
    DisabledPeople,

    [Description("Faith groups")]
    FaithGroups,

    [Description("People at risk of domestic violence")]
    PeopleAtRiskOfDomesticViolence,

    [Description("Young people")]
    YoungPeople,

    [Description("Older people")]
    OlderPeople,

    [Description("None of these groups")]
    NoneOfThese,
}
