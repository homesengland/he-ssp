using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum FacilityType
{
    Undefined = 0,

    [Description("Self-contained facilities")]
    SelfContainedFacilities,

    [Description("Shared facilities")]
    SharedFacilities,

    [Description("Mix of self-contained snd shared facilities")]
    MixOfSelfContainedAndSharedFacilities,
}
