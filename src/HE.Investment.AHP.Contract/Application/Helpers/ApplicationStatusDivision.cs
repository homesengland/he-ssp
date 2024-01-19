using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Application.Helpers;

public static class ApplicationStatusDivision
{
    public static IEnumerable<ApplicationStatus> GetAllStatusesAllowedForWithdraw()
    {
        yield return ApplicationStatus.Draft;
        yield return ApplicationStatus.ApplicationSubmitted;
        yield return ApplicationStatus.OnHold;
        yield return ApplicationStatus.UnderReview;
        yield return ApplicationStatus.ApplicationUnderReview;
        yield return ApplicationStatus.CashflowUnderReview;
        yield return ApplicationStatus.ReferredBackToApplicant;
    }

    public static IEnumerable<ApplicationStatus> GetAllStatusesAllowedForPutOnHold()
    {
        yield return ApplicationStatus.Draft;
        yield return ApplicationStatus.ApplicationSubmitted;
        yield return ApplicationStatus.UnderReview;
        yield return ApplicationStatus.ApplicationUnderReview;
        yield return ApplicationStatus.CashflowUnderReview;
        yield return ApplicationStatus.ReferredBackToApplicant;
    }
}
