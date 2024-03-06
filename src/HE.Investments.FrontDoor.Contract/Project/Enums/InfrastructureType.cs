using System.ComponentModel;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Contract.Project.Enums;

public enum InfrastructureType
{
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
