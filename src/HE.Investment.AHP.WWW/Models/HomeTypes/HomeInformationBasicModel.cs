namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeInformationBasicModel : HomeTypeBasicModel
{
    public HomeInformationBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomeInformationBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? NumberOfHomes { get; set; }

    public string? NumberOfBedrooms { get; set; }

    public string? MaximumOccupancy { get; set; }

    public string? NumberOfStoreys { get; set; }
}
