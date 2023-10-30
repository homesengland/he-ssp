namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RemoveHomeTypeModel : HomeTypeModelBase
{
    public RemoveHomeTypeModel(string applicationName, string homeTypeName)
        : base(applicationName)
    {
        HomeTypeName = homeTypeName;
    }

    public RemoveHomeTypeModel()
        : base(string.Empty)
    {
    }

    public string HomeTypeName { get; set; }
}
