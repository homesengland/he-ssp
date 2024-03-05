using System.ComponentModel;

namespace HE.Investments.FrontDoor.Contract.Project.Enums;

public enum AffordableHomesAmount
{
    Undefined = 0,
    [Description("I want to deliver 100% affordable homes")]
    OnlyAffordableHomes,
    [Description("I want to deliver open market homes and also affordable homes above the amount required by planning")]
    OpenMarkedAndAffordableHomes,
    [Description("I want to deliver open market homes (and the amount of affordable homes required by planning)")]
    OpenMarkedAndRequiredAffordableHomes,
    [Description("I only want to deliver open market homes")]
    OnlyOpenMarketHomes,
    [Description("I do not know")]
    Unknown,
}
