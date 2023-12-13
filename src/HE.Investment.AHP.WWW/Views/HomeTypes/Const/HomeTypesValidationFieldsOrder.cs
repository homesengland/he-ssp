namespace HE.Investment.AHP.WWW.Views.HomeTypes.Const;

public static class HomeTypesValidationFieldsOrder
{
    public static List<string> AffordableRent => new()
    {
        HomeTypesValidationFieldNames.MarketValue,
        HomeTypesValidationFieldNames.MarketRent,
        HomeTypesValidationFieldNames.ProspectiveRent,
        HomeTypesValidationFieldNames.TargetRentExceedMarketRent,
    };

    public static List<string> SocialRent => new()
    {
        HomeTypesValidationFieldNames.MarketValue,
        HomeTypesValidationFieldNames.ProspectiveRent,
    };

    public static List<string> SharedOwnership => new()
    {
        HomeTypesValidationFieldNames.MarketValue,
        HomeTypesValidationFieldNames.InitialSale,
        HomeTypesValidationFieldNames.ProspectiveRent,
    };

    public static List<string> RentToBuy => new()
    {
        HomeTypesValidationFieldNames.MarketValue,
        HomeTypesValidationFieldNames.MarketRent,
        HomeTypesValidationFieldNames.ProspectiveRent,
        HomeTypesValidationFieldNames.TargetRentExceedMarketRent,
    };
}
