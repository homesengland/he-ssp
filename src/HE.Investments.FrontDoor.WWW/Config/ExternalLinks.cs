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

        HomeBuildingFundInfrastructureLoans = configuration.TryGetValue(ConfigurationKey(nameof(HomeBuildingFundInfrastructureLoans))) ??
                                              "https://www.gov.uk/guidance/home-building-fund-infrastructure-loans";
    }

    public string MailToHelpToBuildTechSupport { get; }

    public string HomesEnglandPrivacyNotice { get; }

    public string OpenGovernmentLicence { get; }

    public string UkGovernmentLicensingFramework { get; }

    public string GovUk { get; }

    public string HomeBuildingFundInfrastructureLoans { get; }

    private string ConfigurationKey(string propertyName) => $"{nameof(ExternalLinks)}:{propertyName}";
}
