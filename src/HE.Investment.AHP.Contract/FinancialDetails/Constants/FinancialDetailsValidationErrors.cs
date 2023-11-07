namespace HE.Investment.AHP.Contract.FinancialDetails.Constants;
public static class FinancialDetailsValidationErrors
{
    public const string InvalidPurchasePrice = "There is a problem <br/> The purchase price of the land must be a number";

    public const string InvalidExpectedPurchasePrice = "There is a problem <br/> The expected purchase price of the land must be a number";

    public const string NoLandOwnershipProvided = "Please provide information about land ownership";

    public const string NoLandValue = "Land value is required";

    public const string InvalidLandValue = "Please provide valid land value";
}
