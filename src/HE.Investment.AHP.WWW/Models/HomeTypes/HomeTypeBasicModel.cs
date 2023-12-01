namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeBasicModel : HomeTypeModelBase
{
    public HomeTypeBasicModel(string applicationName, string homeTypeName)
        : base(applicationName)
    {
        HomeTypeName = homeTypeName;
    }

    public string HomeTypeName { get; set; }

    public string Header => $"{ApplicationName} - {HomeTypeName}";
}
