namespace HE.InvestmentLoans.Contract.Application.Enums;

public record ApplicationStatus(string Value)
{
    public static ApplicationStatus Draft => new("draft");

    public static ApplicationStatus Submitted => new("submitted");

    public static ApplicationStatus InDueDiligence => new("in due diligence");

    public static ApplicationStatus ContractSigned => new("contract signed subject to cp");

    public static ApplicationStatus CspSatisfied => new("csp satisfied");

    public static ApplicationStatus LoanAvailable => new("loan available");

    public static ApplicationStatus HoldRequested => new("hold requested");

    public static ApplicationStatus OnHold => new("on hold");

    public static ApplicationStatus ReferredBackToApplicant => new("referred back to applicant");

    public static ApplicationStatus NA => new("n/a");

    public static ApplicationStatus Withdrawn => new("withdrawn");

    public static ApplicationStatus NotApproved => new("not approved");

    public static ApplicationStatus ApplicationDeclined => new("application declined");

    public static ApplicationStatus ApprovedSubjectToContract => new("approved subject to contract");

    public static bool operator ==(string value, ApplicationStatus status)
    {
        if (status is null)
        {
            return false;
        }

        return status.Value.Equals(value, StringComparison.Ordinal);
    }

    public static bool operator !=(string value, ApplicationStatus status)
    {
        if (status is null)
        {
            return false;
        }

        return !status.Value.Equals(value, StringComparison.Ordinal);
    }
}
