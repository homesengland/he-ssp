namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;

internal static class HomeTypePagesUrl
{
    public static string HomeTypeDetails(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "details");

    public static string DisabledPeople(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "homes-for-disabled-people");

    public static string DisabledPeopleClientGroup(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "disabled-people-client-group");

    public static string HappiDesignPrinciples(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "happi-design-principles");

    public static string DesignPlans(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "design-plans");

    public static string SupportedHousingInformation(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "supported-housing-information");

    public static string RevenueFunding(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "revenue-funding");

    public static string MoveOnArrangements(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "move-on-arrangements");

    public static string ExitPlan(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "exit-plan");

    public static string TypologyLocationAndDesign(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "typology-location-and-design");

    public static string PeopleGroupForSpecificDesignFeatures(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "people-group-for-specific-design-features");

    public static string HomeInformation(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "home-information");

    public static string MoveOnAccommodation(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "move-on-accommodation");

    public static string BuildingInformation(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "building-information");

    public static string CustomBuildProperty(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "custom-build-property");

    public static string TypeOfFacilities(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "type-of-facilities");

    public static string AccessibilityStandards(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "accessibility-standards");

    public static string AccessibilityCategory(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "accessibility-category");

    public static string FloorArea(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "floor-area");

    public static string FloorAreaStandards(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "floor-area-standards");

    public static string AffordableRent(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "affordable-rent");

    public static string ExemptFromTheRightToSharedOwnership(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "exempt-from-the-right-to-shared-ownership");

    public static string ExemptionJustification(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "exemption-justification");

    public static string ModernMethodsConstruction(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "modern-methods-construction");

    public static string ModernMethodsConstructionCategories(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "modern-methods-construction-categories");

    public static string ModernMethodsConstruction3DSubcategories(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "modern-methods-construction-3d-subcategories");

    public static string ModernMethodsConstruction2DSubcategories(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "modern-methods-construction-2d-subcategories");

    public static string CheckAnswers(string organisationId, string applicationId, string homeTypeId) =>
        BuildHomeTypePage(organisationId, applicationId, homeTypeId, "check-answers");

    private static string BuildHomeTypePage(string organisationId, string applicationId, string homeTypeId, string pageSuffix)
    {
        return $"ahp/{organisationId}/application/{applicationId}/home-types/{homeTypeId}/{pageSuffix}";
    }
}
