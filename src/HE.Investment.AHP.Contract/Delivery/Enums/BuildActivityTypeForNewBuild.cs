using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum BuildActivityTypeForNewBuild
{
    Undefined = 0,

    [Description("Acquisition and Works")]
    AcquisitionAndWorks,

    [Description("Off The Shelf")]
    OffTheShelf,

    [Description("Works Only")]
    WorksOnly,

    [Description("Land Inclusive Package (package deal)")]
    LandInclusivePackage,

    Regeneration,
}
