namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SitePagesUrl
{
    public static string SiteName(string organisationId) => $"/ahp/{organisationId}/site/name";

    public static string SiteList(string organisationId, string projectId) => $"ahp/{organisationId}/project/{projectId}/sites";

    public static string SiteStart(string organisationId, string siteId) => $"/ahp/{organisationId}/site/{siteId}/start";

    public static string SiteSelect(string organisationId, string projectId, bool? isAfterFdProject = null)
    {
        var url = $"ahp/{organisationId}/site/select?projectid={projectId}";
        if (isAfterFdProject != null)
        {
            url += $"&isafterfdproject={isAfterFdProject}";
        }

        return url;
    }

    public static string SiteDetails(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}";

    public static string SiteSection106GeneralAgreement(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-general-agreement";

    public static string SiteSection106AffordableHousing(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-affordable-housing";

    public static string SiteSection106OnlyAffordableHousing(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-only-affordable-housing";

    public static string SiteSection106AdditionalAffordableHousing(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-additional-affordable-housing";

    public static string SiteSection106CapitalFundingEligibility(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-capital-funding-eligibility";

    public static string SiteSection106LocalAuthorityConfirmation(string organisationId, string siteId) =>
        $"ahp/{organisationId}/site/{siteId}/section-106-local-authority-confirmation";

    public static string SiteLocalAuthoritySearch(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/local-authority/search";

    public static string SiteLocalAuthorityResult(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/local-authority/search/result";

    public static string SiteLocalAuthorityResult(string organisationId, string siteId, string phrase) =>
        $"{SiteLocalAuthorityResult(organisationId, siteId)}?phrase={phrase}";

    public static string SiteLocalAuthorityConfirm(string organisationId, string siteId, string localAuthorityCode, string phrase) =>
        $"ahp/{organisationId}/site/{siteId}/local-authority/{localAuthorityCode}/confirm?phrase={phrase}";

    public static string SiteLocalAuthorityConfirmWithoutQuery(string organisationId, string siteId, string localAuthorityCode) =>
        $"ahp/{organisationId}/site/{siteId}/local-authority/{localAuthorityCode}/confirm";

    public static string SitePlanningStatus(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/planning-status";

    public static string SitePlanningDetails(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/planning-details";

    public static string SiteLandRegistry(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/land-registry";

    public static string SiteNationalDesignGuide(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/national-design-guide";

    public static string SiteBuildingForHealthyLife(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/building-for-a-healthy-life";

    public static string SiteProvideNumberOfGreenLights(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/number-of-green-lights";

    public static string SiteDevelopingPartner(string organisationId, string siteId) => $"ahp/{organisationId}/site-partners/{siteId}/developing-partner";

    public static string SiteDevelopingPartnerConfirmation(string organisationId, string siteId, string partnerId) =>
        $"ahp/{organisationId}/site-partners/{siteId}/developing-partner-confirm/{partnerId}";

    public static string SiteOwnerOfTheLand(string organisationId, string siteId) => $"ahp/{organisationId}/site-partners/{siteId}/owner-of-the-land";

    public static string SiteOwnerOfTheLandConfirmation(string organisationId, string siteId, string partnerId) =>
        $"ahp/{organisationId}/site-partners/{siteId}/owner-of-the-land-confirm/{partnerId}";

    public static string SiteOwnerOfTheHomes(string organisationId, string siteId) => $"ahp/{organisationId}/site-partners/{siteId}/owner-of-the-homes";

    public static string SiteOwnerOfTheHomesConfirmation(string organisationId, string siteId, string partnerId) =>
        $"ahp/{organisationId}/site-partners/{siteId}/owner-of-the-homes-confirm/{partnerId}";

    public static string SiteLandAcquisitionStatus(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/land-acquisition-status";

    public static string SiteConfirm(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/confirm-select";

    public static string SiteTenderingStatus(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/tendering-status";

    public static string SiteContractorDetails(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/contractor-details";

    public static string SiteStrategicSite(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/strategic-site";

    public static string SiteType(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/site-type";

    public static string SiteUse(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/site-use";

    public static string SiteTravellerPitchType(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/traveller-pitch-type";

    public static string SiteRuralClassification(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/rural-classification";

    public static string SiteEnvironmentalImpact(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/environmental-impact";

    public static string SiteMmcUsing(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/mmc-using";

    public static string SiteMmcInformation(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/mmc-information";

    public static string SiteMmcCategories(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/mmc-categories";

    public static string SiteMmcCategory3D(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/mmc-3d-category";

    public static string SiteMmcCategory2D(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/mmc-2d-category";

    public static string SiteProcurements(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/procurements";

    public static string SiteCheckAnswers(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/check-answers";
}
