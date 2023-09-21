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
            ApplicationStatus.Submitted => (int)invln_externalstatus.ApplicationSubmitted,
            ApplicationStatus.InDueDiligence => (int)invln_externalstatus.InDueDiligence,
            ApplicationStatus.ContractSigned => (int)invln_externalstatus.ContractSignedSubjecttoCP,
            ApplicationStatus.CspSatisfied => (int)invln_externalstatus.CPsSatisfied,
            ApplicationStatus.LoanAvailable => (int)invln_externalstatus.LoanAvailable,
            ApplicationStatus.HoldRequested => (int)invln_externalstatus.HoldRequested,
            ApplicationStatus.OnHold => (int)invln_externalstatus.OnHold,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_externalstatus.ReferredBacktoApplicant,
            ApplicationStatus.NA => (int)invln_externalstatus.NA,
            ApplicationStatus.Withdrawn => (int)invln_externalstatus.Withdrawn,
            ApplicationStatus.ApplicationDeclined => (int)invln_externalstatus.ApplicationDeclined,
            ApplicationStatus.ApprovedSubjectToContract => (int)invln_externalstatus.Approvedsubjecttocontract,
            ApplicationStatus.UnderReview => (int)invln_externalstatus.Underreview,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
        return crmStatus switch
        {
            (int)invln_externalstatus.Draft => ApplicationStatus.Draft,
            (int)invln_externalstatus.ApplicationSubmitted => ApplicationStatus.Submitted,
            (int)invln_externalstatus.InDueDiligence => ApplicationStatus.InDueDiligence,
            (int)invln_externalstatus.ContractSignedSubjecttoCP => ApplicationStatus.ContractSigned,
            (int)invln_externalstatus.CPsSatisfied => ApplicationStatus.CspSatisfied,
            (int)invln_externalstatus.LoanAvailable => ApplicationStatus.LoanAvailable,
            (int)invln_externalstatus.HoldRequested => ApplicationStatus.HoldRequested,
            (int)invln_externalstatus.OnHold => ApplicationStatus.OnHold,
            (int)invln_externalstatus.ReferredBacktoApplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_externalstatus.NA => ApplicationStatus.NA,
            (int)invln_externalstatus.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_externalstatus.ApplicationDeclined => ApplicationStatus.ApplicationDeclined,
            (int)invln_externalstatus.Approvedsubjecttocontract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_externalstatus.Underreview => ApplicationStatus.UnderReview,
            null => ApplicationStatus.Draft,
            _ => throw new NotImplementedException(),
        };
    }
}
