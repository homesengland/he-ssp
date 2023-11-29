using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomesForDisabledPeopleBasicModel : HomeTypeBasicModel
{
    public HomesForDisabledPeopleBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomesForDisabledPeopleBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public DisabledPeopleHousingType HousingType { get; set; }
}
