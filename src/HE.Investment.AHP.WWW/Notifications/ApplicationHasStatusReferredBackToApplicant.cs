using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public static class ApplicationHasStatusReferredBackToApplicant
{
    public static DisplayNotification Create()
    {
        return DisplayNotification.Important(
            "Your Growth Manager has referred  application back to you.",
            body: "You can now edit and resubmit your application.");
    }
}
