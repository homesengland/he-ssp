using HE.Investment.AHP.WWW.Extensions;

namespace HE.Investment.AHP.WWW.Config;

public class ExternalLinks : IExternalLinks
{
    public ExternalLinks(IConfiguration configuration)
    {
        BuildingRegulations = configuration.TryGetValue(ConfigurationKey(nameof(BuildingRegulations))) ??
                              "https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/540330/BR_PDF_AD_M1_2015_with_2016_amendments_V3.pdf";

        ApplyForAffordableHousingFunding = configuration.TryGetValue(ConfigurationKey(nameof(ApplyForAffordableHousingFunding))) ??
                                           "https://www.gov.uk/guidance/apply-for-affordable-housing-funding";

        ApplyForAffordableHousingFundingApplySection = configuration.TryGetValue(ConfigurationKey(nameof(ApplyForAffordableHousingFundingApplySection))) ??
                                                       "https://www.gov.uk/guidance/apply-for-affordable-housing-funding#apply";

        GrantAgreementForAhp21To26 = configuration.TryGetValue(ConfigurationKey(nameof(GrantAgreementForAhp21To26))) ??
                                     "https://www.gov.uk/government/publications/grant-agreement-examples-for-the-affordable-homes-programme-2021-to-2026";

        MailToHelpToBuildTechSupport = configuration.TryGetValue(ConfigurationKey(nameof(MailToHelpToBuildTechSupport))) ??
                                       "mailto:helptobuildtechsupport@homesengland.gov.uk";

        CapitalFundingGuide = configuration.TryGetValue(ConfigurationKey(nameof(CapitalFundingGuide))) ??
                              "https://www.gov.uk/guidance/capital-funding-guide/6-programme-management";

        RightToSharedOwnershipInitialGuidanceForRegisteredProviders = configuration.TryGetValue(ConfigurationKey(nameof(RightToSharedOwnershipInitialGuidanceForRegisteredProviders))) ??
                                                                      "https://www.gov.uk/government/publications/right-to-shared-ownership-initial-guidance-for-registered-providers/right-to-shared-ownership-initial-guidance-for-registered-providers";

        MmcIntroduction = configuration.TryGetValue(ConfigurationKey(nameof(MmcIntroduction))) ??
                          "https://www.buildoffsite.com/content/uploads/2019/04/MMC-I-Pad-base_GOVUK-FINAL_SECURE-1.pdf";

        HomesEnglandPrivacyNotice = configuration.TryGetValue(ConfigurationKey(nameof(HomesEnglandPrivacyNotice))) ??
                                    "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan";

        OpenGovernmentLicence = configuration.TryGetValue(ConfigurationKey(nameof(OpenGovernmentLicence))) ??
                                "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";

        UkGovernmentLicensingFramework = configuration.TryGetValue(ConfigurationKey(nameof(UkGovernmentLicensingFramework))) ??
                                         "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";

        BuildingForHealthyLifeBrochure = configuration.TryGetValue(ConfigurationKey(nameof(BuildingForHealthyLifeBrochure))) ??
                                         "https://www.designforhomes.org/wp-content/uploads/2020/11/BFL-2020-Brochure.pdf";

        NationalDesignGuide = configuration.TryGetValue(ConfigurationKey(nameof(NationalDesignGuide))) ??
                              "https://www.gov.uk/government/publications/national-design-guide";

        RegenerationFundingWithin21To26Ahp = configuration.TryGetValue(ConfigurationKey(nameof(RegenerationFundingWithin21To26Ahp))) ??
                                             "https://www.gov.uk/government/news/turbo-boost-for-estate-regeneration-with-major-change-to-the-affordable-homes-programme";

        PolicyStatementOnRentsForSocialHousing = configuration.TryGetValue(ConfigurationKey(nameof(PolicyStatementOnRentsForSocialHousing))) ??
                                                 "https://www.gov.uk/government/publications/direction-on-the-rent-standard-from-1-april-2020/policy-statement-on-rents-for-social-housing";

        SelfBuildAndCustomHousebuilding = configuration.TryGetValue(ConfigurationKey(nameof(SelfBuildAndCustomHousebuilding))) ??
                                          "https://www.gov.uk/guidance/self-build-and-custom-housebuilding";

        NationallyDescribedSpaceStandard = configuration.TryGetValue(ConfigurationKey(nameof(NationallyDescribedSpaceStandard))) ??
                                           "https://www.gov.uk/government/publications/technical-housing-standards-nationally-described-space-standard/technical-housing-standards-nationally-described-space-standard";

        TheHousingOurAgeingPopulationPanelForInnovation = configuration.TryGetValue(ConfigurationKey(nameof(TheHousingOurAgeingPopulationPanelForInnovation))) ??
                                                          "https://www.housinglin.org.uk/Topics/type/The-Housing-our-Ageing-Population-Panel-for-Innovation-HAPPI-Report-2009/";

        GovUk = configuration.TryGetValue(ConfigurationKey(nameof(GovUk))) ?? "https://www.gov.uk/";
    }

    public string BuildingRegulations { get; }

    public string ApplyForAffordableHousingFunding { get; }

    public string ApplyForAffordableHousingFundingApplySection { get; }

    public string GrantAgreementForAhp21To26 { get; }

    public string MailToHelpToBuildTechSupport { get; }

    public string CapitalFundingGuide { get; }

    public string RightToSharedOwnershipInitialGuidanceForRegisteredProviders { get; }

    public string MmcIntroduction { get; }

    public string HomesEnglandPrivacyNotice { get; }

    public string OpenGovernmentLicence { get; }

    public string UkGovernmentLicensingFramework { get; }

    public string BuildingForHealthyLifeBrochure { get; }

    public string NationalDesignGuide { get; }

    public string RegenerationFundingWithin21To26Ahp { get; }

    public string PolicyStatementOnRentsForSocialHousing { get; }

    public string SelfBuildAndCustomHousebuilding { get; }

    public string NationallyDescribedSpaceStandard { get; }

    public string TheHousingOurAgeingPopulationPanelForInnovation { get; }

    public string GovUk { get; }

    private string ConfigurationKey(string propertyName) => $"{nameof(ExternalLinks)}:{propertyName}";
}
