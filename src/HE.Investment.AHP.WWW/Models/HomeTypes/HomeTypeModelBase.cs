namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeModelBase
{
    public HomeTypeModelBase(string applicationName)
    {
        ApplicationName = applicationName;
    }

    public string ApplicationName { get; set; }
}
