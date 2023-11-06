namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeInformationModel : ProvidedHomeTypeModelBase
{
    public HomeInformationModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomeInformationModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string NumberOfHomes { get; set; }

    public string NumberOfBedrooms { get; set; }

    public string MaximumOccupancy { get; set; }

    public string NumberOfStoreys { get; set; }
}
