using System.ComponentModel;

namespace HE.Investments.FrontDoor.Contract.Project;

public enum ActivityType
{
    [Description("Developing homes (including any minor site related infrastructure)")]
    DevelopingHomes,

    [Description("Providing infrastructure")]
    ProvidingInfrastructure,

    [Description("Manufacturing homes within a factory")]
    ManufacturingHomesWithinFactory,

    [Description("Acquiring land")]
    AcquiringLand,

    [Description("Selling land to Homes England")]
    SellingLandToHomesEngland,

    Other,
}
