using HE.Investments.FrontDoor.WWW.Extensions;

namespace HE.Investments.FrontDoor.WWW.Config;

public class ExternalLinks : IExternalLinks
{
    public ExternalLinks(IConfiguration configuration)
    {
        MailToHelpToBuildTechSupport = configuration.TryGetValue(ConfigurationKey(nameof(MailToHelpToBuildTechSupport))) ??
                                       "mailto:helptobuildtechsupport@homesengland.gov.uk";

        HomesEnglandPrivacyNotice = configuration.TryGetValue(ConfigurationKey(nameof(HomesEnglandPrivacyNotice))) ??
                                    "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan";

        OpenGovernmentLicence = configuration.TryGetValue(ConfigurationKey(nameof(OpenGovernmentLicence))) ??
                                "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";

        UkGovernmentLicensingFramework = configuration.TryGetValue(ConfigurationKey(nameof(UkGovernmentLicensingFramework))) ??
                                         "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";

        GovUk = configuration.TryGetValue(ConfigurationKey(nameof(GovUk))) ?? "https://www.gov.uk/";

        MailToEnquiries = configuration.TryGetValue(ConfigurationKey(nameof(MailToEnquiries))) ?? "enquiries@homesengland.gov.uk";

        FindLocalCouncil = configuration.TryGetValue(ConfigurationKey(nameof(FindLocalCouncil))) ?? "https://www.gov.uk/find-local-council";
    }

    public string MailToHelpToBuildTechSupport { get; }

    public string HomesEnglandPrivacyNotice { get; }

    public string OpenGovernmentLicence { get; }

    public string UkGovernmentLicensingFramework { get; }

    public string GovUk { get; }

    public string MailToEnquiries { get; }

    public string FindLocalCouncil { get; }

    private string ConfigurationKey(string propertyName) => $"{nameof(ExternalLinks)}:{propertyName}";
}
