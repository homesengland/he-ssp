namespace HE.Investments.Common.Models.App;

/// <summary>
/// Configuration settings related to Cookie Authentication and timeouts.
/// </summary>
public class CookieAuthenticationConfig
{
    /// <summary>
    /// Gets or sets the number of minutes before the authentication session cookie expires.
    /// </summary>
    public int ExpireMinutes { get; set; }

    /// <summary>
    /// Gets or sets the number of minutes prior to authentication session cookie expiration that we show the expiration warning dialog.
    /// </summary>
    public int SessionExpireNotificationMinutes { get; set; }
}
