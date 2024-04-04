using System.ComponentModel;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Shared.Project.Contract;

public enum InfrastructureType
{
    Unknown = 0,

    [Description("Site preparation")]
    SitePreparation,

    [Description("Enabling")]
    Enabling,

    [Description("Physical infrastructure")]
    [Hint("This could include but is not limited to transport and travel, utilities, schools, community and healthcare facilities, and green and blue infrastructure.")]
    PhysicalInfrastructure,

    [Description("Other")]
    Other,

    [Description("I do not know")]
    IDoNotKnow,
}
