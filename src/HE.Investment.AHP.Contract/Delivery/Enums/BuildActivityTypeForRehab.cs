using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum BuildActivityTypeForRehab
{
    Undefined = 0,

    [Description("Acquisition and Works Rehab")]
    AcquisitionAndWorksRehab,

    [Description("Existing satisfactory ")]
    ExistingSatisfactory,

    [Description("Purchase and repair")]
    PurchaseAndRepair,

    [Description("Lease and repair")]
    LeaseAndRepair,

    Reimprovement,

    Conversion,

    [Description("Works Only")]
    WorksOnly,

    Regeneration,
}
