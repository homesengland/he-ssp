using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.WWW.Common;

public static class ExternalLinks
{
    public const string BuildingRegulations =
        "https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/540330/BR_PDF_AD_M1_2015_with_2016_amendments_V3.pdf";

    public const string ApplyForAffordableHousingFunding =
        "https://www.gov.uk/guidance/apply-for-affordable-housing-funding";

    public const string ApplyForAffordableHousingFundingApplySection =
        "https://www.gov.uk/guidance/apply-for-affordable-housing-funding#apply";

    public const string GrantAgreementForAhp21To26 =
        "https://www.gov.uk/government/publications/grant-agreement-examples-for-the-affordable-homes-programme-2021-to-2026";

    public const string MailToHelpToBuildTechSupport =
        "mailto:helptobuildtechsupport@homesengland.gov.uk";

    public const string CapitalFundingGuide =
        "https://www.gov.uk/guidance/capital-funding-guide/6-programme-management";

    public const string RightToSharedOwnershipInitialGuidanceForRegisteredProviders =
        "https://www.gov.uk/government/publications/right-to-shared-ownership-initial-guidance-for-registered-providers/right-to-shared-ownership-initial-guidance-for-registered-providers";

    public const string MmcIntroduction =
        "https://www.buildoffsite.com/content/uploads/2019/04/MMC-I-Pad-base_GOVUK-FINAL_SECURE-1.pdf";

    public const string HomesEnglandPrivacyNotice =
        "https://www.gov.uk/government/publications/homes-england-privacy-notice/homes-england-privacy-notice#help-to-build-equity-loan";

    public const string OpenGovernmentLicence =
        "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";

    public const string UkGovernmentLicensingFramework =
        "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";

    public const string BuildingForHealthyLifeBrochure =
        "https://www.designforhomes.org/wp-content/uploads/2020/11/BFL-2020-Brochure.pdf";

    public const string NationalDesignGuide =
        "https://www.gov.uk/government/publications/national-design-guide";

    public const string RegenerationFundingWithin21To26Ahp =
        "https://www.gov.uk/government/news/turbo-boost-for-estate-regeneration-with-major-change-to-the-affordable-homes-programme";

    public const string PolicyStatementOnRentsForSocialHousing =
        "https://www.gov.uk/government/publications/direction-on-the-rent-standard-from-1-april-2020/policy-statement-on-rents-for-social-housing";

    public const string SelfBuildAndCustomHousebuilding =
        "https://www.gov.uk/guidance/self-build-and-custom-housebuilding";

    public const string NationallyDescribedSpaceStandard =
        "https://www.gov.uk/government/publications/technical-housing-standards-nationally-described-space-standard/technical-housing-standards-nationally-described-space-standard";

    public const string TheHousingOurAgeingPopulationPanelForInnovation =
        "https://www.housinglin.org.uk/Topics/type/The-Housing-our-Ageing-Population-Panel-for-Innovation-HAPPI-Report-2009/";

    public const string GovUk =
        "https://www.gov.uk/";

    private static IConfiguration _configuration;

    public static void SetConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string Get(string linkName)
    {
        var link = TryGetValueFromConfig(linkName) ?? TryGetValueFromField(linkName);

        return link ?? throw new NotFoundException($"Link not found for {linkName}");
    }

    private static string? TryGetValueFromConfig(string key)
    {
        try
        {
            return _configuration.GetValue<string>(key);
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    private static string? TryGetValueFromField(string fieldName)
    {
        var fieldInfo = typeof(ExternalLinks).GetField(fieldName);
        return fieldInfo?.GetValue(null)?.ToString();
    }
}
