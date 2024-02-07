using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site.Enums;

public enum BuildingForHealthyLifeType
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No")]
    No,

    [Description("Not applicable, this site does not contain more than 10 homes")]
    NotApplicable,
}
