namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class SocialRentModel : HomeTypeBasicModel
{
    public SocialRentModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public SocialRentModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? MarketRent { get; set; }
}
