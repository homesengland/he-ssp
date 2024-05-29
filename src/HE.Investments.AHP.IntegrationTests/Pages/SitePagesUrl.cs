namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SitePagesUrl
{
    public const string SiteName = "/ahp/site/name";

    public static string SiteList(string projectId) => $"ahp/project/{projectId}/sites";

    public static string SiteStart(string siteId) => $"/ahp/site/{siteId}/start";

    public static string SiteSelect(string projectId) => $"ahp/site/select?projectid={projectId}";

    public static string SiteDetails(string siteId) => $"ahp/site/{siteId}";

    public static string SiteSection106GeneralAgreement(string siteId) => $"ahp/site/{siteId}/section-106-general-agreement";

    public static string SiteSection106AffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-affordable-housing";

    public static string SiteSection106OnlyAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-only-affordable-housing";

    public static string SiteSection106AdditionalAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-additional-affordable-housing";

    public static string SiteSection106CapitalFundingEligibility(string siteId) => $"ahp/site/{siteId}/section-106-capital-funding-eligibility";

    public static string SiteSection106LocalAuthorityConfirmation(string siteId) => $"ahp/site/{siteId}/section-106-local-authority-confirmation";

    public static string SiteLocalAuthoritySearch(string siteId) => $"ahp/site/{siteId}/local-authority/search";

    public static string SiteLocalAuthorityResult(string siteId) => $"ahp/site/{siteId}/local-authority/search/result";

    public static string SiteLocalAuthorityResult(string siteId, string phrase) => $"{SiteLocalAuthorityResult(siteId)}?phrase={phrase}";

    public static string SiteLocalAuthorityConfirm(string siteId, string localAuthorityCode, string phrase) =>
        $"ahp/site/{siteId}/local-authority/{localAuthorityCode}/confirm?phrase={phrase}";

    public static string SiteLocalAuthorityConfirmWithoutQuery(string siteId, string localAuthorityCode) =>
        $"ahp/site/{siteId}/local-authority/{localAuthorityCode}/confirm";

    public static string SitePlanningStatus(string siteId) => $"ahp/site/{siteId}/planning-status";

    public static string SitePlanningDetails(string siteId) => $"ahp/site/{siteId}/planning-details";

    public static string SiteLandRegistry(string siteId) => $"ahp/site/{siteId}/land-registry";

    public static string SiteNationalDesignGuide(string siteId) => $"ahp/site/{siteId}/national-design-guide";

    public static string SiteBuildingForHealthyLife(string siteId) => $"ahp/site/{siteId}/building-for-a-healthy-life";

    public static string SiteProvideNumberOfGreenLights(string siteId) => $"ahp/site/{siteId}/number-of-green-lights";

    public static string SiteDevelopingPartner(string siteId) => $"ahp/site/{siteId}/developing-partner";

    public static string SiteDevelopingPartnerConfirmation(string siteId, string organisationId) => $"ahp/site/{siteId}/developing-partner-confirm/{organisationId}";

    public static string SiteOwnerOfTheLand(string siteId) => $"ahp/site/{siteId}/owner-of-the-land";

    public static string SiteOwnerOfTheLandConfirmation(string siteId, string organisationId) => $"ahp/site/{siteId}/owner-of-the-land-confirm/{organisationId}";

    public static string SiteOwnerOfTheHomes(string siteId) => $"ahp/site/{siteId}/owner-of-the-homes";

    public static string SiteOwnerOfTheHomesConfirmation(string siteId, string organisationId) => $"ahp/site/{siteId}/owner-of-the-homes-confirm/{organisationId}";

    public static string SiteLandAcquisitionStatus(string siteId) => $"ahp/site/{siteId}/land-acquisition-status";

    public static string SiteConfirm(string siteId) => $"ahp/site/{siteId}/confirm-select";

    public static string SiteTenderingStatus(string siteId) => $"ahp/site/{siteId}/tendering-status";

    public static string SiteContractorDetails(string siteId) => $"ahp/site/{siteId}/contractor-details";

    public static string SiteStrategicSite(string siteId) => $"ahp/site/{siteId}/strategic-site";

    public static string SiteType(string siteId) => $"ahp/site/{siteId}/site-type";

    public static string SiteUse(string siteId) => $"ahp/site/{siteId}/site-use";

    public static string SiteTravellerPitchType(string siteId) => $"ahp/site/{siteId}/traveller-pitch-type";

    public static string SiteRuralClassification(string siteId) => $"ahp/site/{siteId}/rural-classification";

    public static string SiteEnvironmentalImpact(string siteId) => $"ahp/site/{siteId}/environmental-impact";

    public static string SiteMmcUsing(string siteId) => $"ahp/site/{siteId}/mmc-using";

    public static string SiteMmcInformation(string siteId) => $"ahp/site/{siteId}/mmc-information";

    public static string SiteMmcCategories(string siteId) => $"ahp/site/{siteId}/mmc-categories";

    public static string SiteMmcCategory3D(string siteId) => $"ahp/site/{siteId}/mmc-3d-category";

    public static string SiteMmcCategory2D(string siteId) => $"ahp/site/{siteId}/mmc-2d-category";

    public static string SiteProcurements(string siteId) => $"ahp/site/{siteId}/procurements";

    public static string SiteCheckAnswers(string siteId) => $"ahp/site/{siteId}/check-answers";
}
