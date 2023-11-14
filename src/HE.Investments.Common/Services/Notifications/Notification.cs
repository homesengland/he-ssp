namespace HE.Investments.Common.Services.Notifications;

public class Notification
{
    public Notification(string notificationType, IDictionary<string, string> parameters)
    {
        NotificationType = notificationType;
        Parameters = parameters;
    }

    protected Notification(IDictionary<string, string>? parameters = null)
    {
        NotificationType = GetType().Name;
        Parameters = parameters ?? new Dictionary<string, string>();
    }

    public string NotificationType { get; }

    public IDictionary<string, string> Parameters { get; }

    public string TemplateText(string text)
    {
        var templatedText = text;
        foreach (var parameter in Parameters)
        {
            templatedText = templatedText.Replace($"<{parameter.Key}>", parameter.Value);
        }

        return templatedText;
    }
}
