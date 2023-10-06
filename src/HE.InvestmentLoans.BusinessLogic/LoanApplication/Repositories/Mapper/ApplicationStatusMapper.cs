using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
public class ApplicationStatusMapper
{
    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => (int)invln_externalstatus.Draft,
            ApplicationStatus.ApplicationSubmitted => (int)invln_externalstatus.ApplicationSubmitted,
            ApplicationStatus.InDueDiligence => (int)invln_externalstatus.InDueDiligence,
            ApplicationStatus.ContractSigned => (int)invln_externalstatus.ContractSignedSubjecttoCP,
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
        return crmStatus switch
        {
            (int)invln_externalstatus.Draft => ApplicationStatus.Draft,
            (int)invln_externalstatus.ApplicationSubmitted => ApplicationStatus.ApplicationSubmitted,
            (int)invln_externalstatus.InDueDiligence => ApplicationStatus.InDueDiligence,
            (int)invln_externalstatus.ContractSignedSubjecttoCP => ApplicationStatus.ContractSigned,
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
    }
}
