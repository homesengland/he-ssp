using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site;

public enum SiteTenderingStatus
{
    Undefined,
    [Description("Unconditional works contract let or works being provided by in house team")]
    UnconditionalWorksContract,
    [Description("Conditional contract let or partner identified but not yet in contract")]
    ConditionalWorksContract,
    [Description("Tender for works contract out for competition")]
    TenderForWorksContract,
    [Description("Works contracting process has not yet begun")]
    ContractingHasNotYetBegun,
    [Description("Not applicable, works not required")]
    NotApplicable,
}
