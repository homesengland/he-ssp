namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class ProspectiveRentIneligibleModel : HomeTypeBasicModel
{
    public ProspectiveRentIneligibleModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public ProspectiveRentIneligibleModel()
        : base(string.Empty, string.Empty)
    {
    }

    public string BackAction { get; set; }
}
