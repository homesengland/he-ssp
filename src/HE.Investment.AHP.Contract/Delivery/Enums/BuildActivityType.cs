using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum BuildActivityType
{
    Undefined = 0,

    [Description("Acquisition and Works")]
    AcquisitionAndWorks,

    [Description("Off The Shelf")]
    OffTheShelf,

    [Description("Works Only")]
    WorksOnly,

    [Description("Works Only")]
    WorksOnlyRehab,

    [Description("Land Inclusive Package (package deal)")]
    LandInclusivePackage,

    [Description("Acquisition and Works Rehab")]
    AcquisitionAndWorksRehab,

    [Description("Existing satisfactory ")]
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
