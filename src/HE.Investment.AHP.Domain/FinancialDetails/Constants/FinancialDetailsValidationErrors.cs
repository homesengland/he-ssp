namespace HE.Investment.AHP.Domain.FinancialDetails.Constants;
public static class FinancialDetailsValidationErrors
{
    public const string InvalidPurchasePrice = "There is a problem <br/> The purchase price of the land must be a number";

    public const string InvalidExpectedPurchasePrice = "There is a problem <br/> The expected purchase price of the land must be a number";

    public const string NoLandOwnershipProvided = "Please provide information about land ownership";

    public const string NoLandValue = "Land value is required";

    public const string InvalidLandValue = "There is a problem <br/> The current value of the land must be a number";

    public const string InvalidExpectedWorksCosts = "There is a problem <br/> The expected works costs must be a whole number, like 300";

    public const string InvalidExpectedOnCosts = "There is a problem <br/> The expected on costs must be a whole number, like 300";
}
