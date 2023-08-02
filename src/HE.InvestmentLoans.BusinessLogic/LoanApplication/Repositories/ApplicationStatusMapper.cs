using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
public class ApplicationStatusMapper
{
    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => ApplicationCrmStatus.Draft,
            ApplicationStatus.Submitted => ApplicationCrmStatus.Submitted,
            ApplicationStatus.InDueDiligence => ApplicationCrmStatus.InDueDiligence,
            ApplicationStatus.ContractSigned => ApplicationCrmStatus.ContractSigned,
            ApplicationStatus.CspSatisfied => ApplicationCrmStatus.CspSatisfied,
            ApplicationStatus.LoanAvailable => ApplicationCrmStatus.LoanAvailable,
            ApplicationStatus.HoldRequested => ApplicationCrmStatus.HoldRequested,
            ApplicationStatus.OnHold => ApplicationCrmStatus.OnHold,
            ApplicationStatus.ReferredBackToApplicant => ApplicationCrmStatus.ReferredBackToApplicant,
            ApplicationStatus.NA => ApplicationCrmStatus.NA,
            ApplicationStatus.Withdrawn => ApplicationCrmStatus.Withdrawn,
            ApplicationStatus.NotApproved => ApplicationCrmStatus.NotApproved,
            ApplicationStatus.ApplicationDeclined => ApplicationCrmStatus.ApplicationDeclined,
            ApplicationStatus.ApprovedSubjectToContract => ApplicationCrmStatus.ApprovedSubjectToContract,
            ApplicationStatus.UnderReview => ApplicationCrmStatus.Underreview,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
        return crmStatus switch
        {
            ApplicationCrmStatus.Draft => ApplicationStatus.Draft,
            ApplicationCrmStatus.Submitted => ApplicationStatus.Submitted,
            ApplicationCrmStatus.InDueDiligence => ApplicationStatus.InDueDiligence,
            ApplicationCrmStatus.ContractSigned => ApplicationStatus.ContractSigned,
            ApplicationCrmStatus.CspSatisfied => ApplicationStatus.CspSatisfied,
            ApplicationCrmStatus.LoanAvailable => ApplicationStatus.LoanAvailable,
            ApplicationCrmStatus.HoldRequested => ApplicationStatus.HoldRequested,
            ApplicationCrmStatus.OnHold => ApplicationStatus.OnHold,
            ApplicationCrmStatus.ReferredBackToApplicant => ApplicationStatus.ReferredBackToApplicant,
            ApplicationCrmStatus.NA => ApplicationStatus.NA,
            ApplicationCrmStatus.Withdrawn => ApplicationStatus.Withdrawn,
            ApplicationCrmStatus.NotApproved => ApplicationStatus.NotApproved,
            ApplicationCrmStatus.ApplicationDeclined => ApplicationStatus.ApplicationDeclined,
            ApplicationCrmStatus.ApprovedSubjectToContract => ApplicationStatus.ApprovedSubjectToContract,
            ApplicationCrmStatus.Underreview => ApplicationStatus.UnderReview,
            null => ApplicationStatus.Draft,
            _ => throw new NotImplementedException(),
        };
    }
}
