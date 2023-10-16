using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.Contract.Application.Helper;

public static class ApplicationStatusDivision
{
    public static IEnumerable<ApplicationStatus> GetAllStatusesForReadonlyMode()
    {
        yield return ApplicationStatus.CashflowRequested;
        yield return ApplicationStatus.CashflowUnderReview;
        yield return ApplicationStatus.ReferredBackToApplicant;
        yield return ApplicationStatus.UnderReview;
        yield return ApplicationStatus.SentForApproval;
        yield return ApplicationStatus.ApprovedSubjectToDueDiligence;
        yield return ApplicationStatus.ApplicationDeclined;
        yield return ApplicationStatus.InDueDiligence;
        yield return ApplicationStatus.ApprovedSubjectToContract;
        yield return ApplicationStatus.AwaitingCpSatisfaction;
        yield return ApplicationStatus.CpsSatisfied;
        yield return ApplicationStatus.LoanAvailable;
        yield return ApplicationStatus.HoldRequested;
        yield return ApplicationStatus.OnHold;
        yield return ApplicationStatus.Withdrawn;
    }

    public static IEnumerable<ApplicationStatus> GetAllStatusesAfterSubmit()
    {
        yield return ApplicationStatus.ApplicationSubmitted;
        yield return ApplicationStatus.ApplicationUnderReview;
        yield return ApplicationStatus.HoldRequested;
        yield return ApplicationStatus.OnHold;
        yield return ApplicationStatus.CashflowRequested;
        yield return ApplicationStatus.CashflowUnderReview;
        yield return ApplicationStatus.ReferredBackToApplicant;
        yield return ApplicationStatus.UnderReview;
        yield return ApplicationStatus.SentForApproval;
        yield return ApplicationStatus.NotApproved;
        yield return ApplicationStatus.ApprovedSubjectToDueDiligence;
        yield return ApplicationStatus.InDueDiligence;
        yield return ApplicationStatus.SentForPreCompleteApproval;
        yield return ApplicationStatus.ApprovedSubjectToContract;
        yield return ApplicationStatus.AwaitingCpSatisfaction;
        yield return ApplicationStatus.CpsSatisfied;
        yield return ApplicationStatus.Withdrawn;
    }

    public static IEnumerable<ApplicationStatus> GetAllStatusesAllowedForWithdraw()
    {
        yield return ApplicationStatus.ApplicationSubmitted;
        yield return ApplicationStatus.ApplicationUnderReview;
        yield return ApplicationStatus.HoldRequested;
        yield return ApplicationStatus.OnHold;
        yield return ApplicationStatus.CashflowRequested;
        yield return ApplicationStatus.CashflowUnderReview;
        yield return ApplicationStatus.ReferredBackToApplicant;
        yield return ApplicationStatus.UnderReview;
        yield return ApplicationStatus.SentForApproval;
        yield return ApplicationStatus.NotApproved;
        yield return ApplicationStatus.ApprovedSubjectToDueDiligence;
        yield return ApplicationStatus.InDueDiligence;
        yield return ApplicationStatus.SentForPreCompleteApproval;
        yield return ApplicationStatus.ApprovedSubjectToContract;
        yield return ApplicationStatus.AwaitingCpSatisfaction;
        yield return ApplicationStatus.CpsSatisfied;
        yield return ApplicationStatus.Draft;
    }
}
