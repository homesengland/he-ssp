namespace HE.Investments.Common.Models.App;

public class CrmApiConfig : ICrmApiConfig
{
    public string? BaseUri { get; set; }

    public string? SubscriptionKey { get; set; }

    public int RetryCount { get; set; } = 1;

    public int RetryDelayInMilliseconds { get; set; } = 250;
}
