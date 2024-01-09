namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class HomeTypePagesUrl
{
    public static string HomeInformation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "HomeInformation");

    public static string MoveOnAccommodation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "MoveOnAccommodation");

    public static string BuildingInformation(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "BuildingInformation");

    public static string CustomBuildProperty(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "CustomBuildProperty");

    public static string TypeOfFacilities(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "TypeOfFacilities");

    public static string AccessibilityStandards(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "AccessibilityStandards");

    public static string AccessibilityCategory(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "AccessibilityCategory");

    public static string FloorArea(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "FloorArea");

    public static string FloorAreaStandards(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "FloorAreaStandards");

    public static string AffordableRent(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "AffordableRent");

    public static string ExemptFromTheRightToSharedOwnership(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "ExemptFromTheRightToSharedOwnership");

    public static string ExemptionJustification(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "ExemptionJustification");

    public static string ModernMethodsConstruction(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "ModernMethodsConstruction");

    public static string ModernMethodsConstructionCategories(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "ModernMethodsConstructionCategories");

    public static string CheckAnswers(string applicationId, string homeTypeId) => BuildHomeTypePage(applicationId, homeTypeId, "CheckAnswers");

    private static string BuildHomeTypePage(string applicationId, string homeTypeId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/HomeTypes/{homeTypeId}/{pageSuffix}";
    }
}
