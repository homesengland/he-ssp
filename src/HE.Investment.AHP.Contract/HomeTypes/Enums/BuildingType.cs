using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum BuildingType
{
    Undefined = 0,

    [Description("House")]
    House,

    [Description("Flat")]
    Flat,

    [Description("Bedsit")]
    Bedsit,

    [Description("Bungalow")]
    Bungalow,

    [Description("Maisonette")]
    Maisonette,
}
