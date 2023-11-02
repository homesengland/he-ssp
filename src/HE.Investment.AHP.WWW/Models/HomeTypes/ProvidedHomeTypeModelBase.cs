namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class ProvidedHomeTypeModelBase : HomeTypeModelBase
{
    public ProvidedHomeTypeModelBase(string applicationName, string homeTypeName)
        : base(applicationName)
    {
        HomeTypeName = homeTypeName;
    }

    public string HomeTypeName { get; set; }

    public string Header => string.IsNullOrEmpty(HomeTypeName) ? ApplicationName : $"{ApplicationName} - {HomeTypeName}";
}
