namespace HE.InvestmentLoans.Common.Models.App;

public class NotificationConfig
{
    /// <summary>
    /// Gets or sets the api key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the client name.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Gets or sets the domain.
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// Gets or sets the SubmissionConfirmationTemplateId.
    /// </summary>
    public Dictionary<string, string>? Templates { get; set; }
}
