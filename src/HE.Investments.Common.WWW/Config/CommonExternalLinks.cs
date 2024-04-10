using Microsoft.Extensions.Configuration;

namespace HE.Investments.Common.WWW.Config;

public class CommonExternalLinks
{
    private readonly IConfiguration _configuration;

    protected CommonExternalLinks(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GovUk => GetExternalLink(nameof(GovUk), "https://www.gov.uk/");

    public string MailToHelpToBuildTechSupport => GetExternalLink(
        nameof(MailToHelpToBuildTechSupport),
        "mailto:helptobuildtechsupport@homesengland.gov.uk");

    public string HomesEnglandPrivacyNotice => GetExternalLink(
        nameof(HomesEnglandPrivacyNotice),
        "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan");

    public string OpenGovernmentLicence => GetExternalLink(
        nameof(OpenGovernmentLicence),
        "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/");

    public string UkGovernmentLicensingFramework => GetExternalLink(
        nameof(UkGovernmentLicensingFramework),
        "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/");

    protected string GetExternalLink(string linkName, string defaultValue) =>
        _configuration.GetValue<string>($"AppConfiguration:ExternalLinks:{linkName}") ?? defaultValue;
}
