using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomesForOlderPeopleModel : ProvidedHomeTypeModelBase
{
    public HomesForOlderPeopleModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomesForOlderPeopleModel()
        : this(string.Empty, string.Empty)
    {
    }

    public OlderPeopleHousingType HousingType { get; set; }
}
