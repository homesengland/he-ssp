using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.Common.CRM;

public static class ApplicationStatusMapper
{
    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => (int)invln_externalstatus.Draft,
            ApplicationStatus.ApplicationSubmitted => (int)invln_externalstatus.ApplicationSubmitted,
            ApplicationStatus.InDueDiligence => (int)invln_externalstatus.InDueDiligence,
            ApplicationStatus.AwaitingCpSatisfaction => (int)invln_externalstatus.ContractSignedSubjecttoCP,
            ApplicationStatus.CpsSatisfied => (int)invln_externalstatus.CPsSatisfied,
            ApplicationStatus.LoanAvailable => (int)invln_externalstatus.LoanAvailable,
            ApplicationStatus.HoldRequested => (int)invln_externalstatus.HoldRequested,
            ApplicationStatus.OnHold => (int)invln_externalstatus.OnHold,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_externalstatus.ReferredBacktoApplicant,
            ApplicationStatus.NA => (int)invln_externalstatus.NA,
            ApplicationStatus.Withdrawn => (int)invln_externalstatus.Withdrawn,
            ApplicationStatus.ApplicationDeclined => (int)invln_externalstatus.ApplicationDeclined,
            ApplicationStatus.ApprovedSubjectToContract => (int)invln_externalstatus.ApprovedSubjecttoContract,
            ApplicationStatus.UnderReview => (int)invln_externalstatus.UnderReview,
            ApplicationStatus.ApplicationUnderReview => (int)invln_externalstatus.ApplicationUnderReview,
            ApplicationStatus.CashflowRequested => (int)invln_externalstatus.CashflowRequested,
            ApplicationStatus.CashflowUnderReview => (int)invln_externalstatus.CashflowUnderReview,
            ApplicationStatus.SentForApproval => (int)invln_externalstatus.SentforApproval,
            ApplicationStatus.ApprovedSubjectToDueDiligence => (int)invln_externalstatus.ApprovedSubjecttoDueDiligence,
            ApplicationStatus.New => (int)invln_externalstatus.New,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        return crmStatus switch
        {
            // TODO: remove this line when AHP application status mapping will be finished. Once this TODO is removed, also remove the PRAGMA.
            1 => ApplicationStatus.Draft,
            2 => ApplicationStatus.Draft,

            (int)invln_externalstatus.Draft => ApplicationStatus.Draft,
            (int)invln_externalstatus.ApplicationSubmitted => ApplicationStatus.ApplicationSubmitted,
            (int)invln_externalstatus.InDueDiligence => ApplicationStatus.InDueDiligence,
            (int)invln_externalstatus.ContractSignedSubjecttoCP => ApplicationStatus.AwaitingCpSatisfaction,
            (int)invln_externalstatus.CPsSatisfied => ApplicationStatus.CpsSatisfied,
            (int)invln_externalstatus.LoanAvailable => ApplicationStatus.LoanAvailable,
            (int)invln_externalstatus.HoldRequested => ApplicationStatus.HoldRequested,
            (int)invln_externalstatus.OnHold => ApplicationStatus.OnHold,
            (int)invln_externalstatus.ReferredBacktoApplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_externalstatus.NA => ApplicationStatus.NA,
            (int)invln_externalstatus.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_externalstatus.ApplicationDeclined => ApplicationStatus.ApplicationDeclined,
            (int)invln_externalstatus.ApprovedSubjecttoContract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_externalstatus.UnderReview => ApplicationStatus.UnderReview,
            (int)invln_externalstatus.ApplicationUnderReview => ApplicationStatus.ApplicationUnderReview,
            (int)invln_externalstatus.CashflowRequested => ApplicationStatus.CashflowRequested,
            (int)invln_externalstatus.CashflowUnderReview => ApplicationStatus.CashflowUnderReview,
            (int)invln_externalstatus.SentforApproval => ApplicationStatus.SentForApproval,
            (int)invln_externalstatus.ApprovedSubjecttoDueDiligence => ApplicationStatus.ApprovedSubjectToDueDiligence,
            (int)invln_externalstatus.New => ApplicationStatus.New,
            null => ApplicationStatus.Draft,
            _ => throw new ArgumentOutOfRangeException(nameof(crmStatus), crmStatus, null),
        };
#pragma warning restore S1135 // Track uses of "TODO" tags
    }
}

