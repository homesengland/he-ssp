namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeInformationModel : HomeTypeModelBase
{
    public HomeInformationModel(string applicationName)
        : base(applicationName)
    {
    }

    public HomeInformationModel()
        : this(string.Empty)
    {
    }

    public string HomeTypeName { get; set; }

    public string NumberOfHomes { get; set; }

    public string NumberOfBedrooms { get; set; }

    public string MaximumOccupancy { get; set; }

    public string NumberOfStoreys { get; set; }

    public string HomeInformationHeader => string.IsNullOrEmpty(HomeTypeName) ? ApplicationName : $"{ApplicationName} - {HomeTypeName}";
}
