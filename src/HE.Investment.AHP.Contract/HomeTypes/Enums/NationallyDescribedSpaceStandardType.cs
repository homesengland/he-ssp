using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum NationallyDescribedSpaceStandardType
{
    Undefined = 0,

    [Description("Built-in storage space size")]
    BuiltInStorageSpaceSize,

    [Description("Bedroom areas")]
    BedroomAreas,

    [Description("Bedroom widths")]
    BedroomWidth,

    [Description("None of these")]
    NoneOfThese,
}
