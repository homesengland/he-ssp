namespace HE.Investments.Loans.Common.Models.App;

/// <summary>
/// ApplicationInsightsConfig.
/// </summary>
public class ApplicationInsightsConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether application insights should be enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether application insights cookies should be enabled.
    /// </summary>
    public bool CookiesEnabled { get; set; }

    /// <summary>
    /// Gets or sets the instrumentation key.
    /// </summary>
    public string? InstrumentationKey { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets user role to send from JS to App Insights custom property of page view telemetry.
    /// </summary>
    public string? UserRole { get; set; }
}
