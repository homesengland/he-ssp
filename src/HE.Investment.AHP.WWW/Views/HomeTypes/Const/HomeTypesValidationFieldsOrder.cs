using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Views.HomeTypes.Const;

public static class HomeTypesValidationFieldsOrder
{
    public static List<string> AffordableRent => new()
    {
        nameof(AffordableRentModel.MarketValue),
        nameof(AffordableRentModel.MarketRent),
        nameof(AffordableRentModel.ProspectiveRent),
        nameof(AffordableRentModel.ProspectiveRentAsPercentageOfMarketRent),
        nameof(AffordableRentModel.TargetRentExceedMarketRent),
    };

    public static List<string> SocialRent => new()
    {
        nameof(SocialRentModel.MarketValue),
        nameof(SocialRentModel.ProspectiveRent),
    };

    public static List<string> SharedOwnership => new()
    {
        nameof(SharedOwnershipModel.MarketValue),
        nameof(SharedOwnershipModel.InitialSale),
        nameof(SharedOwnershipModel.ExpectedFirstTranche),
        nameof(SharedOwnershipModel.ProspectiveRent),
        nameof(SharedOwnershipModel.RentAsPercentageOfTheUnsoldShare),
    };

    public static List<string> RentToBuy => new()
    {
        nameof(RentToBuyModel.MarketValue),
        nameof(RentToBuyModel.MarketRent),
        nameof(RentToBuyModel.ProspectiveRent),
        nameof(RentToBuyModel.ProspectiveRentAsPercentageOfMarketRent),
        nameof(RentToBuyModel.TargetRentExceedMarketRent),
    };

    public static List<string> HomeOwnershipDisabilities => new()
    {
        nameof(HomeOwnershipDisabilitiesModel.MarketValue),
        nameof(HomeOwnershipDisabilitiesModel.InitialSale),
        nameof(HomeOwnershipDisabilitiesModel.ExpectedFirstTranche),
        nameof(HomeOwnershipDisabilitiesModel.ProspectiveRent),
        nameof(HomeOwnershipDisabilitiesModel.RentAsPercentageOfTheUnsoldShare),
    };

    public static List<string> HomeInformation => new()
    {
        nameof(HomeInformationModel.NumberOfHomes),
        nameof(HomeInformationModel.NumberOfBedrooms),
        nameof(HomeInformationModel.MaximumOccupancy),
        nameof(HomeInformationModel.NumberOfStoreys),
    };
}
