using System.ComponentModel;

namespace HE.InvestmentLoans.Contract.Application.Enums;

public enum ApplicationStatus
{
    New,
    Draft,
    [Description("Application Submitted")]
    Submitted,
    NA,
    [Description("Application Under Review")]
    UnderReview,
    Withdrawn,

    [Description("Hold Requested")]
    HoldRequested,
    [Description("On Hold")]
    OnHold,

    InDueDiligence,
    ContractSigned,
    CspSatisfied,
    LoanAvailable,
    ReferredBackToApplicant,
    NotApproved,
    ApplicationDeclined,
    ApprovedSubjectToContract,
}
