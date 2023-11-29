namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeBasicModel : ProvidedHomeTypeModelBase
{
    public HomeTypeBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomeTypeBasicModel()
        : this(string.Empty, string.Empty)
    {
    }
}
