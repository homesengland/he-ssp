using HE.Investments.Common.WWW.Config;

namespace HE.Investment.AHP.WWW.Config;

public class AhpExternalLinks : CommonExternalLinks, IAhpExternalLinks
{
    public AhpExternalLinks(IConfiguration configuration)
        : base(configuration)
    {
    }

    public string BuildingRegulations => GetExternalLink(
        nameof(BuildingRegulations),
        "https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/540330/BR_PDF_AD_M1_2015_with_2016_amendments_V3.pdf");

    public string ApplyForAffordableHousingFunding => GetExternalLink(
        nameof(ApplyForAffordableHousingFunding),
        "https://www.gov.uk/guidance/apply-for-affordable-housing-funding");

    public string ApplyForAffordableHousingFundingApplySection => GetExternalLink(
        nameof(ApplyForAffordableHousingFundingApplySection),
        "https://www.gov.uk/guidance/apply-for-affordable-housing-funding#apply");

    public string GrantAgreementForAhp21To26 => GetExternalLink(
        nameof(GrantAgreementForAhp21To26),
        "https://www.gov.uk/government/publications/grant-agreement-examples-for-the-affordable-homes-programme-2021-to-2026");

    public string CapitalFundingGuide => GetExternalLink(
        nameof(CapitalFundingGuide),
        "https://www.gov.uk/guidance/capital-funding-guide");

    public string CapitalFundingGuideProgrammeManagement => GetExternalLink(
        nameof(CapitalFundingGuideProgrammeManagement),
        "https://www.gov.uk/guidance/capital-funding-guide/6-programme-management");

    public string RightToSharedOwnershipInitialGuidanceForRegisteredProviders => GetExternalLink(
        nameof(RightToSharedOwnershipInitialGuidanceForRegisteredProviders),
        "https://www.gov.uk/government/publications/right-to-shared-ownership-initial-guidance-for-registered-providers/right-to-shared-ownership-initial-guidance-for-registered-providers");

    public string MmcIntroduction => GetExternalLink(
        nameof(MmcIntroduction),
        "https://www.buildoffsite.com/content/uploads/2019/04/MMC-I-Pad-base_GOVUK-FINAL_SECURE-1.pdf");

    public string BuildingForHealthyLife => GetExternalLink(
        nameof(BuildingForHealthyLife),
        "https://www.gov.uk/government/collections/building-healthy-places");

    public string NationalDesignGuide => GetExternalLink(
        nameof(NationalDesignGuide),
        "https://www.gov.uk/government/publications/national-design-guide");

    public string RegenerationFundingWithin21To26Ahp => GetExternalLink(
        nameof(RegenerationFundingWithin21To26Ahp),
        "https://www.gov.uk/government/news/turbo-boost-for-estate-regeneration-with-major-change-to-the-affordable-homes-programme");

    public string PolicyStatementOnRentsForSocialHousing => GetExternalLink(
        nameof(PolicyStatementOnRentsForSocialHousing),
        "https://www.gov.uk/government/publications/direction-on-the-rent-standard-from-1-april-2020/policy-statement-on-rents-for-social-housing");

    public string SelfBuildAndCustomHousebuilding => GetExternalLink(
        nameof(SelfBuildAndCustomHousebuilding),
        "https://www.gov.uk/guidance/self-build-and-custom-housebuilding");

    public string NationallyDescribedSpaceStandard => GetExternalLink(
        nameof(NationallyDescribedSpaceStandard),
        "https://www.gov.uk/government/publications/technical-housing-standards-nationally-described-space-standard/technical-housing-standards-nationally-described-space-standard");

    public string TheHousingOurAgeingPopulationPanelForInnovation => GetExternalLink(
        nameof(TheHousingOurAgeingPopulationPanelForInnovation),
        "https://www.housinglin.org.uk/Topics/type/The-Housing-our-Ageing-Population-Panel-for-Innovation-HAPPI-Report-2009/");
}
