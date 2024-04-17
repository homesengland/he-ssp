using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.Common.CRM.Mappers;

public static class ApplicationStatusMapper
{
    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => (int)invln_ExternalStatus.Draft,
            ApplicationStatus.ApplicationSubmitted => (int)invln_ExternalStatus.ApplicationSubmitted,
            ApplicationStatus.InDueDiligence => (int)invln_ExternalStatus.InDueDiligence,
            ApplicationStatus.AwaitingCpSatisfaction => (int)invln_ExternalStatus.ContractSignedSubjecttoCP,
            ApplicationStatus.ConditionsSatisfied => (int)invln_ExternalStatus.CPsSatisfied,
            ApplicationStatus.LoanAvailable => (int)invln_ExternalStatus.LoanAvailable,
            ApplicationStatus.HoldRequested => (int)invln_ExternalStatus.HoldRequested,
            ApplicationStatus.OnHold => (int)invln_ExternalStatus.OnHold,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_ExternalStatus.ReferredBacktoApplicant,
            ApplicationStatus.NA => (int)invln_ExternalStatus.NA,
            ApplicationStatus.Withdrawn => (int)invln_ExternalStatus.Withdrawn,
            ApplicationStatus.ApplicationDeclined => (int)invln_ExternalStatus.ApplicationDeclined,
            ApplicationStatus.ApprovedSubjectToContract => (int)invln_ExternalStatus.ApprovedSubjecttoContract,
            ApplicationStatus.UnderReview => (int)invln_ExternalStatus.UnderReview,
            ApplicationStatus.ApplicationUnderReview => (int)invln_ExternalStatus.ApplicationUnderReview,
            ApplicationStatus.CashflowRequested => (int)invln_ExternalStatus.CashflowRequested,
            ApplicationStatus.CashflowUnderReview => (int)invln_ExternalStatus.CashflowUnderReview,
            ApplicationStatus.SentForApproval => (int)invln_ExternalStatus.SentforApproval,
            ApplicationStatus.ApprovedSubjectToDueDiligence => (int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence,
            ApplicationStatus.New => (int)invln_ExternalStatus.New,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
        return crmStatus switch
        {
            (int)invln_ExternalStatus.Draft => ApplicationStatus.Draft,
            (int)invln_ExternalStatus.ApplicationSubmitted => ApplicationStatus.ApplicationSubmitted,
            (int)invln_ExternalStatus.InDueDiligence => ApplicationStatus.InDueDiligence,
            (int)invln_ExternalStatus.ContractSignedSubjecttoCP => ApplicationStatus.AwaitingCpSatisfaction,
            (int)invln_ExternalStatus.ConditionsSatisfied => ApplicationStatus.ConditionsSatisfied,
            (int)invln_ExternalStatus.LoanAvailable => ApplicationStatus.LoanAvailable,
            (int)invln_ExternalStatus.HoldRequested => ApplicationStatus.HoldRequested,
            (int)invln_ExternalStatus.OnHold => ApplicationStatus.OnHold,
            (int)invln_ExternalStatus.ReferredBacktoApplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_ExternalStatus.NA => ApplicationStatus.NA,
            (int)invln_ExternalStatus.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_ExternalStatus.ApplicationDeclined => ApplicationStatus.ApplicationDeclined,
            (int)invln_ExternalStatus.ApprovedSubjecttoContract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_ExternalStatus.UnderReview => ApplicationStatus.UnderReview,
            (int)invln_ExternalStatus.ApplicationUnderReview => ApplicationStatus.ApplicationUnderReview,
            (int)invln_ExternalStatus.CashflowRequested => ApplicationStatus.CashflowRequested,
            (int)invln_ExternalStatus.CashflowUnderReview => ApplicationStatus.CashflowUnderReview,
            (int)invln_ExternalStatus.SentforApproval => ApplicationStatus.SentForApproval,
            (int)invln_ExternalStatus.ApprovedSubjecttoDueDiligence => ApplicationStatus.ApprovedSubjectToDueDiligence,
            (int)invln_ExternalStatus.New => ApplicationStatus.New,
            null => ApplicationStatus.Draft,
            _ => throw new ArgumentOutOfRangeException(nameof(crmStatus), crmStatus, null),
        };
    }
}
