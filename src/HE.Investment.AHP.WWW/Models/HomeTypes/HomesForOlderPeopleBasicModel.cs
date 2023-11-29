using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomesForOlderPeopleBasicModel : HomeTypeBasicModel
{
    public HomesForOlderPeopleBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomesForOlderPeopleBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public OlderPeopleHousingType HousingType { get; set; }
}
