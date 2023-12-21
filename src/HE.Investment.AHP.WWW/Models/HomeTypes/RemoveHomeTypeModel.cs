using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RemoveHomeTypeModel : HomeTypeBasicModel
{
    public RemoveHomeTypeModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RemoveHomeTypeModel()
        : base(string.Empty, string.Empty)
    {
    }

    public RemoveHomeTypeAnswer RemoveHomeTypeAnswer { get; set; }
}
