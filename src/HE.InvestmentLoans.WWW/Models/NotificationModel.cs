using Microsoft.AspNetCore.Html;

namespace HE.InvestmentLoans.WWW.Models;

public class NotificationModel
{
    public NotificationModel(string title, IHtmlContent description, bool isTypeSuccess = false)
    {
        Title = title;
        Description = description;
        IsTypeSuccess = isTypeSuccess;
    }

    public string Title { get; private set; }

    public IHtmlContent Description { get; private set; }

    public bool IsTypeSuccess { get; private set; }
}
