namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RemoveHomeTypeBasicModel : HomeTypeBasicModel
{
    public RemoveHomeTypeBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RemoveHomeTypeBasicModel()
        : base(string.Empty, string.Empty)
    {
    }
}
