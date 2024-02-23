namespace HE.Investment.AHP.WWW.Config;

public class ExternalLinks : IExternalLinks
{
    public ExternalLinks(IConfiguration configuration)
    {
        BuildingRegulations = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(BuildingRegulations))) ??
                              "https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/540330/BR_PDF_AD_M1_2015_with_2016_amendments_V3.pdf";

        ApplyForAffordableHousingFunding = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(ApplyForAffordableHousingFunding))) ??
                                           "https://www.gov.uk/guidance/apply-for-affordable-housing-funding";

        ApplyForAffordableHousingFundingApplySection = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(ApplyForAffordableHousingFundingApplySection))) ??
                                                       "https://www.gov.uk/guidance/apply-for-affordable-housing-funding#apply";

        GrantAgreementForAhp21To26 = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(GrantAgreementForAhp21To26))) ??
                                     "https://www.gov.uk/government/publications/grant-agreement-examples-for-the-affordable-homes-programme-2021-to-2026";

        MailToHelpToBuildTechSupport = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(MailToHelpToBuildTechSupport))) ??
                                       "mailto:helptobuildtechsupport@homesengland.gov.uk";

        CapitalFundingGuide = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(CapitalFundingGuide))) ??
                              "https://www.gov.uk/guidance/capital-funding-guide/6-programme-management";

        RightToSharedOwnershipInitialGuidanceForRegisteredProviders = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(RightToSharedOwnershipInitialGuidanceForRegisteredProviders))) ??
                                                                      "https://www.gov.uk/government/publications/right-to-shared-ownership-initial-guidance-for-registered-providers/right-to-shared-ownership-initial-guidance-for-registered-providers";

        MmcIntroduction = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(MmcIntroduction))) ??
                          "https://www.buildoffsite.com/content/uploads/2019/04/MMC-I-Pad-base_GOVUK-FINAL_SECURE-1.pdf";

        HomesEnglandPrivacyNotice = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(HomesEnglandPrivacyNotice))) ??
                                    "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan";

        OpenGovernmentLicence = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(OpenGovernmentLicence))) ??
                                "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";

        UkGovernmentLicensingFramework = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(UkGovernmentLicensingFramework))) ??
                                         "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";

        BuildingForHealthyLifeBrochure = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(BuildingForHealthyLifeBrochure))) ??
                                         "https://www.designforhomes.org/wp-content/uploads/2020/11/BFL-2020-Brochure.pdf";

        NationalDesignGuide = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(NationalDesignGuide))) ??
                              "https://www.gov.uk/government/publications/national-design-guide";

        RegenerationFundingWithin21To26Ahp = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(RegenerationFundingWithin21To26Ahp))) ??
                                             "https://www.gov.uk/government/news/turbo-boost-for-estate-regeneration-with-major-change-to-the-affordable-homes-programme";

        PolicyStatementOnRentsForSocialHousing = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(PolicyStatementOnRentsForSocialHousing))) ??
                                                 "https://www.gov.uk/government/publications/direction-on-the-rent-standard-from-1-april-2020/policy-statement-on-rents-for-social-housing";

        SelfBuildAndCustomHousebuilding = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(SelfBuildAndCustomHousebuilding))) ??
                                          "https://www.gov.uk/guidance/self-build-and-custom-housebuilding";

        NationallyDescribedSpaceStandard = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(NationallyDescribedSpaceStandard))) ??
                                           "https://www.gov.uk/government/publications/technical-housing-standards-nationally-described-space-standard/technical-housing-standards-nationally-described-space-standard";

        TheHousingOurAgeingPopulationPanelForInnovation = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(TheHousingOurAgeingPopulationPanelForInnovation))) ??
                                                          "https://www.housinglin.org.uk/Topics/type/The-Housing-our-Ageing-Population-Panel-for-Innovation-HAPPI-Report-2009/";

        GovUk = ConfigurationRetriever.TryGetValue(configuration, ConfigurationKey(nameof(GovUk))) ?? "https://www.gov.uk/";
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

    public string ConfigurationKey(string propertyName) => $"{nameof(ExternalLinks)}:{propertyName}";
}
