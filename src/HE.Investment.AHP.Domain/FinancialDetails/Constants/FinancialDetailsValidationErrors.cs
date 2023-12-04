namespace HE.Investment.AHP.Domain.FinancialDetails.Constants;
public static class FinancialDetailsValidationErrors
{
    public const string InvalidActualPurchasePrice = "The purchase price of the land must be a number";

    public const string NoPurchasePrice = "The purchase price of the land was not provided";

    public const string InvalidExpectedPurchasePrice = "The expected purchase price of the land must be a number";

    public const string NoLandOwnershipProvided = "Please provide information about land ownership";

    public const string NoLandValue = "Land value is required";

    public const string InvalidLandValue = "The current value of the land must be a number";

    public const string InvalidExpectedWorksCosts = "The expected works costs must be a whole number, like 300";

    public const string InvalidExpectedOnCosts = "The expected on costs must be a whole number, like 300";

    public const string GenericAmountValidationError = "The amount must be a whole number, like 300";

    public const string CostsAndFundingMismatch = "The total scheme costs and total contributions must match";
}
