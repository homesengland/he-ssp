namespace HE.Investment.AHP.WWW.Config;

public class ExternalLinks : IExternalLinks
{
    public ExternalLinks(IConfiguration configuration)
    {
        BuildingRegulations = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                              "https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/540330/BR_PDF_AD_M1_2015_with_2016_amendments_V3.pdf";

        ApplyForAffordableHousingFunding = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                           "https://www.gov.uk/guidance/apply-for-affordable-housing-funding";

        ApplyForAffordableHousingFundingApplySection = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                                       "https://www.gov.uk/guidance/apply-for-affordable-housing-funding#apply";

        GrantAgreementForAhp21To26 = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                     "https://www.gov.uk/government/publications/grant-agreement-examples-for-the-affordable-homes-programme-2021-to-2026";

        MailToHelpToBuildTechSupport = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                       "mailto:helptobuildtechsupport@homesengland.gov.uk";

        CapitalFundingGuide = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                              "https://www.gov.uk/guidance/capital-funding-guide/6-programme-management";

        RightToSharedOwnershipInitialGuidanceForRegisteredProviders = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                                                      "https://www.gov.uk/government/publications/right-to-shared-ownership-initial-guidance-for-registered-providers/right-to-shared-ownership-initial-guidance-for-registered-providers";

        MmcIntroduction = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                          "https://www.buildoffsite.com/content/uploads/2019/04/MMC-I-Pad-base_GOVUK-FINAL_SECURE-1.pdf";

        HomesEnglandPrivacyNotice = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                    "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan";

        OpenGovernmentLicence = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";

        UkGovernmentLicensingFramework = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                         "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";

        BuildingForHealthyLifeBrochure = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                         "https://www.designforhomes.org/wp-content/uploads/2020/11/BFL-2020-Brochure.pdf";

        NationalDesignGuide = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                              "https://www.gov.uk/government/publications/national-design-guide";

        RegenerationFundingWithin21To26Ahp = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                             "https://www.gov.uk/government/news/turbo-boost-for-estate-regeneration-with-major-change-to-the-affordable-homes-programme";

        PolicyStatementOnRentsForSocialHousing = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                                 "https://www.gov.uk/government/publications/direction-on-the-rent-standard-from-1-april-2020/policy-statement-on-rents-for-social-housing";

        SelfBuildAndCustomHousebuilding = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                          "https://www.gov.uk/guidance/self-build-and-custom-housebuilding";

        NationallyDescribedSpaceStandard = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                           "https://www.gov.uk/government/publications/technical-housing-standards-nationally-described-space-standard/technical-housing-standards-nationally-described-space-standard";

        TheHousingOurAgeingPopulationPanelForInnovation = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ??
                                                          "https://www.housinglin.org.uk/Topics/type/The-Housing-our-Ageing-Population-Panel-for-Innovation-HAPPI-Report-2009/";

        GovUk = configuration.GetValue<string>("AppConfiguration:ExternalLinks") ?? "https://www.gov.uk/";
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
}
