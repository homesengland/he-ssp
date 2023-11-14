namespace HE.Investment.AHP.Domain.FinancialDetails.Constants;
public static class FinancialDetailsValidationErrors
{
    public const string InvalidPurchasePrice = "The purchase price of the land must be a number";

    public const string InvalidExpectedPurchasePrice = "The expected purchase price of the land must be a number";

    public const string NoLandOwnershipProvided = "Please provide information about land ownership";

    public const string NoLandValue = "Land value is required";

    public const string InvalidLandValue = "The current value of the land must be a number";

    public const string InvalidExpectedWorksCosts = "The expected works costs must be a whole number, like 300";

    public const string InvalidExpectedOnCosts = "The expected on costs must be a whole number, like 300";
}
