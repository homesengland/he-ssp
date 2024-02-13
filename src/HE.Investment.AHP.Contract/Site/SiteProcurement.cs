using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site;

public enum SiteProcurement
{
    Undefined = 0,

    [Description("Large scale contract procurement (as individual provider)")]
    LargeScaleContractProcurementAsIndividualProvider,

    [Description("Large scale contract procurement (through consortium)")]
    LargeScaleContractProcurementThroughConsortium,

    [Description("Bulk purchase of components")]
    BulkPurchaseOfComponents,

    [Description("Partnering supply chain")]
    PartneringSupplyChain,

    [Description("Partnering arrangements with contractor")]
    PartneringArrangementsWithContractor,

    Other,

    None,
}
