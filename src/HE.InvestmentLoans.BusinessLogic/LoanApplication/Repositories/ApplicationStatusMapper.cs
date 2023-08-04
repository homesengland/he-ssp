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
            ApplicationStatus.Submitted => (int)invln_externalstatus.Submitted,
            ApplicationStatus.InDueDiligence => (int)invln_externalstatus.Induediligence,
            ApplicationStatus.ContractSigned => (int)invln_externalstatus.ContractSignedsubjecttoCP,
            ApplicationStatus.CspSatisfied => (int)invln_externalstatus.CPssatisfied,
            ApplicationStatus.LoanAvailable => (int)invln_externalstatus.Loanavailable,
            ApplicationStatus.HoldRequested => (int)invln_externalstatus.Holdrequested,
            ApplicationStatus.OnHold => (int)invln_externalstatus.Onhold,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_externalstatus.Referredbacktoapplicant,
            ApplicationStatus.NA => (int)invln_externalstatus.Inactive,
            ApplicationStatus.Withdrawn => (int)invln_externalstatus.Withdrawn,
            ApplicationStatus.NotApproved => (int)invln_externalstatus.Notapproved,
            ApplicationStatus.ApplicationDeclined => (int)invln_externalstatus.Applicationdeclined,
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
            (int)invln_externalstatus.Submitted => ApplicationStatus.Submitted,
            (int)invln_externalstatus.Induediligence => ApplicationStatus.InDueDiligence,
            (int)invln_externalstatus.ContractSignedsubjecttoCP => ApplicationStatus.ContractSigned,
            (int)invln_externalstatus.CPssatisfied => ApplicationStatus.CspSatisfied,
            (int)invln_externalstatus.Loanavailable => ApplicationStatus.LoanAvailable,
            (int)invln_externalstatus.Holdrequested => ApplicationStatus.HoldRequested,
            (int)invln_externalstatus.Onhold => ApplicationStatus.OnHold,
            (int)invln_externalstatus.Referredbacktoapplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_externalstatus.Inactive => ApplicationStatus.NA,
            (int)invln_externalstatus.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_externalstatus.Notapproved => ApplicationStatus.NotApproved,
            (int)invln_externalstatus.Applicationdeclined => ApplicationStatus.ApplicationDeclined,
            (int)invln_externalstatus.Approvedsubjecttocontract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_externalstatus.Underreview => ApplicationStatus.UnderReview,
            null => ApplicationStatus.Draft,
            _ => throw new NotImplementedException(),
        };
    }
}
