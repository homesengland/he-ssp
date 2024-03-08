using System.ComponentModel;

namespace HE.Investments.FrontDoor.Contract.Project.Enums;

public enum RegionType
{
    Undefined = 0,
    [Description("North East")]
    NorthEast,
    [Description("North West")]
    NorthWest,
    [Description("Yorkshire and the Humber")]
    YorkshireAndTheHumber,
    [Description("East Midlands")]
    EastMidlands,
    [Description("West Midlands")]
    WestMidlands,
    [Description("East of England")]
    EastOfEngland,
    [Description("South East")]
    SouthEast,
    [Description("South West")]
    SouthWest,
    [Description("London")]
    London,
}
