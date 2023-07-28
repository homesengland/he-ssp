using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
public class ApplicationStatusMapper
{
    public static string MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => ApplicationStatusString.Draft,
            ApplicationStatus.Submitted => ApplicationStatusString.Submitted,
            ApplicationStatus.InDueDiligence => ApplicationStatusString.InDueDiligence,
            ApplicationStatus.ContractSigned => ApplicationStatusString.ContractSigned,
            ApplicationStatus.CspSatisfied => ApplicationStatusString.CspSatisfied,
            ApplicationStatus.LoanAvailable => ApplicationStatusString.LoanAvailable,
            ApplicationStatus.HoldRequested => ApplicationStatusString.HoldRequested,
            ApplicationStatus.OnHold => ApplicationStatusString.OnHold,
            ApplicationStatus.ReferredBackToApplicant => ApplicationStatusString.ReferredBackToApplicant,
            ApplicationStatus.NA => ApplicationStatusString.NA,
            ApplicationStatus.Withdrawn => ApplicationStatusString.Withdrawn,
            ApplicationStatus.NotApproved => ApplicationStatusString.NotApproved,
            ApplicationStatus.ApplicationDeclined => ApplicationStatusString.ApplicationDeclined,
            ApplicationStatus.ApprovedSubjectToContract => ApplicationStatusString.ApprovedSubjectToContract,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(string crmStatus)
    {
        return crmStatus switch
        {
            ApplicationStatusString.Draft => ApplicationStatus.Draft,
            ApplicationStatusString.Submitted => ApplicationStatus.Submitted,
            ApplicationStatusString.InDueDiligence => ApplicationStatus.InDueDiligence,
            ApplicationStatusString.ContractSigned => ApplicationStatus.ContractSigned,
            ApplicationStatusString.CspSatisfied => ApplicationStatus.CspSatisfied,
            ApplicationStatusString.LoanAvailable => ApplicationStatus.LoanAvailable,
            ApplicationStatusString.HoldRequested => ApplicationStatus.HoldRequested,
            ApplicationStatusString.OnHold => ApplicationStatus.OnHold,
            ApplicationStatusString.ReferredBackToApplicant => ApplicationStatus.ReferredBackToApplicant,
            ApplicationStatusString.NA => ApplicationStatus.NA,
            ApplicationStatusString.Withdrawn => ApplicationStatus.Withdrawn,
            ApplicationStatusString.NotApproved => ApplicationStatus.NotApproved,
            ApplicationStatusString.ApplicationDeclined => ApplicationStatus.ApplicationDeclined,
            ApplicationStatusString.ApprovedSubjectToContract => ApplicationStatus.ApprovedSubjectToContract,
            null => ApplicationStatus.Draft,
            _ => throw new NotImplementedException(),
        };
    }
}
