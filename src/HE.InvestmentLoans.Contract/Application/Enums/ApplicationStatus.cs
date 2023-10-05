using System.ComponentModel;

namespace HE.InvestmentLoans.Contract.Application.Enums;

public enum ApplicationStatus
{
    New,
    Draft,
    [Description("Application Submitted")]
    ApplicationSubmitted,
    NA,
    [Description("Under Review")]
    UnderReview,
    [Description("Application Under Review")]
    ApplicationUnderReview,
    Withdrawn,
    [Description("Hold Requested")]
    HoldRequested,
    [Description("On Hold")]
    OnHold,
    [Description("Cashflow Requested")]
    CashflowRequested,
    [Description("Cashflow Under Review")]
    CashflowUnderReview,
    [Description("Referred Back to Applicant")]
    ReferredBackToApplicant,
    [Description("Sent for Approval")]
    SentForApproval,
    [Description("Approved Subject to Due Diligence")]
    ApprovedSubjectToDueDiligence,
    [Description("Application Declined")]
    ApplicationDeclined,
    [Description("In Due Diligence")]
    InDueDiligence,
    [Description("Approved Subject to Contract")]
    ApprovedSubjectToContract,
    [Description("Contract Signed Subject to CP")]
    ContractSigned,
    [Description("CPs Satisfied")]
    CpsSatisfied,
    [Description("Loan Available")]
    LoanAvailable,
    [Description("Not Approved")]
    NotApproved,
    [Description("Sent for Pre-Complete Approval")]
    SentForPreCompleteApproval,
    [Description("Awaiting CP Satisfaction")]
    AwaitingCpSatisfaction,
}
