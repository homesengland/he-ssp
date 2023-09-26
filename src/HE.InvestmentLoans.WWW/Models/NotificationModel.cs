namespace HE.InvestmentLoans.WWW.Models;

public class NotificationModel
{
    public NotificationModel(string title, string description, bool isTypeSuccess = false)
    {
        Title = title;
        Description = description;
        IsTypeSuccess = isTypeSuccess;
    }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public bool IsTypeSuccess { get; private set; }

    public string IsTypeSuccessNotification()
    {
        if (IsTypeSuccess)
        {
            return "govuk-notification-banner--success";
        }

        return string.Empty;
    }
}
