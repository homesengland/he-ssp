using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.Site.Enums;

public enum SiteLandAcquisitionStatus
{
    Undefined = 0,

    [Description("Full unconditional ownership, acquisition of freehold or long leasehold interest has occurred")]
    FullOwnership,

    [Description("Land being gifted or provided at a discount by the local authority")]
    LandGifted,

    [Description("Conditional acquisition, land option or heads of terms")]
    [Hint("For sites being acquired through golden brick arrangements, select this option.")]
    ConditionalAcquisition,

    [Description("Land purchase negotiations underway")]
    PurchaseNegotiationInProgress,

    [Description("Land identified but purchase negotiations not yet started")]
    PurchaseNegotiationsNotStarted,
}
