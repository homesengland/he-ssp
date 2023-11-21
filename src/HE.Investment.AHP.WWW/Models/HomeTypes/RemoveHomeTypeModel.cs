namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RemoveHomeTypeModel : ProvidedHomeTypeModelBase
{
    public RemoveHomeTypeModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RemoveHomeTypeModel()
        : base(string.Empty, string.Empty)
    {
    }
}
