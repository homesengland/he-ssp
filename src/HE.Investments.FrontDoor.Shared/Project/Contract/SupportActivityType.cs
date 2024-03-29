using System.ComponentModel;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Shared.Project.Contract;

public enum SupportActivityType
{
    [Description("Developing homes (including any minor site related infrastructure)")]
    [Hint("This includes new build and regeneration.")]
    DevelopingHomes = 1,

    [Description("Providing infrastructure")]
    [Hint("Support would be provided directly for the provision of infrastructure only (such as remediation, access or utilities) and not for the direct development of housing.")]
    ProvidingInfrastructure,

    [Description("Manufacturing homes within a factory")]
    ManufacturingHomesWithinFactory,

    [Description("Acquiring land")]
    AcquiringLand,

    [Description("Selling land to Homes England")]
    SellingLandToHomesEngland,

    Other,
}
