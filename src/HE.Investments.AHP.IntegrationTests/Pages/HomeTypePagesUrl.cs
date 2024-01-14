namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class HomeTypePagesUrl
{
    public static string HomeInformation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "home-information");

    public static string MoveOnAccommodation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "move-on-accommodation");

    public static string BuildingInformation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "building-information");

    public static string CustomBuildProperty(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "custom-build-property");

    public static string TypeOfFacilities(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "type-of-facilities");

    public static string AccessibilityStandards(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "accessibility-standards");

    public static string AccessibilityCategory(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "accessibility-category");

    public static string FloorArea(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "floor-area");

    public static string FloorAreaStandards(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "floor-area-standards");

    public static string AffordableRent(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "affordable-rent");

    public static string ExemptFromTheRightToSharedOwnership(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "exempt-from-the-right-to-shared-ownership");

    public static string ExemptionJustification(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "exemption-justification");

    public static string ModernMethodsConstruction(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "modern-methods-construction");

    public static string ModernMethodsConstructionCategories(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "modern-methods-construction-categories");

    public static string CheckAnswers(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "check-answers");

    private static string BuildHomeTypePage(string applicationId, string homeTypeId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/home-types/{homeTypeId}/{pageSuffix}";
    }
}
