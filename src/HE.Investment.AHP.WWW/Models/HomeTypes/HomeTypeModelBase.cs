namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeModelBase
{
    public HomeTypeModelBase(string schemeName)
    {
        SchemeName = schemeName;
    }

    public string SchemeName { get; set; }
}
