namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeOwnershipDisabilitiesModel : HomeTypeBasicModel
{
    public HomeOwnershipDisabilitiesModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomeOwnershipDisabilitiesModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? InitialSale { get; set; }

    public string? ExpectedFirstTranche { get; set; }

    public string? ProspectiveRent { get; set; }

    public string? RentAsPercentageOfTheUnsoldShare { get; set; }
}
