namespace HE.InvestmentLoans.Common.Models.App;

/// <summary>
/// FeedBackUrl Configurations.
/// </summary>
public interface IUrlConfig
{
    /// <summary>
    /// Gets or sets the Guidance url.
    /// </summary>
    public string? Guidance { get; set; }

    /// <summary>
    /// Gets or sets the Feedback Url.
    /// </summary>
    public string? Feedback { get; set; }

    /// <summary>
    /// Gets or sets the Smart Survey Url.
    /// </summary>
    public string? SmartSurvey { get; set; }
}

public class UrlConfig : IUrlConfig
{
    public string? Guidance { get; set; }

    public string? Feedback { get; set; }

    public string? SmartSurvey { get; set; }
}
