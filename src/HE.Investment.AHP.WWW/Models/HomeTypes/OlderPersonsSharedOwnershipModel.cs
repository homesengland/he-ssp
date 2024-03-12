namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class OlderPersonsSharedOwnershipModel : HomeTypeBasicModel
{
    public OlderPersonsSharedOwnershipModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public OlderPersonsSharedOwnershipModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? InitialSale { get; set; }

    public string? ExpectedFirstTranche { get; set; }

    public string? RentPerWeek { get; set; }

    public string? RentAsPercentageOfTheUnsoldShare { get; set; }
}
