namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class SharedOwnershipModel : HomeTypeBasicModel
{
    public SharedOwnershipModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public SharedOwnershipModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? InitialSale { get; set; }

    public string? ExpectedFirstTranche { get; set; }

    public string? ProspectiveRent { get; set; }

    public string? SharedOwnershipRentAsPercentageOfTheUnsoldShare { get; set; }
}
