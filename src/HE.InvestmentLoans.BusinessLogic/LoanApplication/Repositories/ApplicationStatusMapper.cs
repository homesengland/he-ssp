using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
public class ApplicationStatusMapper
{
    public string MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.Draft => "draft",
            ApplicationStatus.Submitted => "submitted",
            ApplicationStatus.InDueDiligence => "in due diligence",
            ApplicationStatus.ContractSigned => "contract signed subject to cp",
            ApplicationStatus.CspSatisfied => "csp satisfied",
            ApplicationStatus.LoanAvailable => "loan available",
            ApplicationStatus.HoldRequested => "hold requested",
            ApplicationStatus.OnHold => "on hold",
            ApplicationStatus.ReferredBackToApplicant => "referred back to applicant",
            ApplicationStatus.NA => "n/a",
            ApplicationStatus.Withdrawn => "withdrawn",
            ApplicationStatus.NotApproved => "not approved",
            ApplicationStatus.ApplicationDeclined => "application declined",
            ApplicationStatus.ApprovedSubjectToContract => "approved subject to contract",
            _ => throw new NotImplementedException(),
        };
    }

    public ApplicationStatus MapToPortalStatus(string crmStatus)
    {
        return crmStatus switch
        {
            "draft" => ApplicationStatus.Draft,
            "submitted" => ApplicationStatus.Submitted,
            "in due diligence" => ApplicationStatus.InDueDiligence,
            "contract signed subject to cp" => ApplicationStatus.ContractSigned,
            "csp satisfied" => ApplicationStatus.CspSatisfied,
            "loan available" => ApplicationStatus.LoanAvailable,
            "hold requested" => ApplicationStatus.HoldRequested,
            "on hold" => ApplicationStatus.OnHold,
            "referred back to applicant" => ApplicationStatus.ReferredBackToApplicant,
            "n/a" => ApplicationStatus.NA,
            "withdrawn" => ApplicationStatus.Withdrawn,
            "not approved" => ApplicationStatus.NotApproved,
            "application declined" => ApplicationStatus.ApplicationDeclined,
            "approved subject to contract" => ApplicationStatus.ApprovedSubjectToContract,
            null => ApplicationStatus.Draft,
            _ => throw new NotImplementedException(),
        };
    }
}
