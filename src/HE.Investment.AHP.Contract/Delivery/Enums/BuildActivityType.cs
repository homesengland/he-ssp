using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum BuildActivityType
{
    Undefined = 0,

    [Description("Acquisition and works")]
    AcquisitionAndWorks,

    [Description("Off the shelf")]
    OffTheShelf,

    [Description("Works only")]
    WorksOnly,

    [Description("Works only")]
    WorksOnlyRehab,

    [Description("Land inclusive package (package deal)")]
    LandInclusivePackage,

    [Description("Acquisition and works (rehab)")]
    AcquisitionAndWorksRehab,

    [Description("Existing satisfactory")]
    ExistingSatisfactory,

    [Description("Purchase and repair")]
    PurchaseAndRepair,

    [Description("Lease and repair")]
    LeaseAndRepair,

    Regeneration,

    [Description("Regeneration")]
    RegenerationRehab,

    [Description("Re-improvement")]
    Reimprovement,

    Conversion,
}
