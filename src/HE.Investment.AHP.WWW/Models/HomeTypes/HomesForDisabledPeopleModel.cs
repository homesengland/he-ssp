using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomesForDisabledPeopleModel : ProvidedHomeTypeModelBase
{
    public HomesForDisabledPeopleModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomesForDisabledPeopleModel()
        : this(string.Empty, string.Empty)
    {
    }

    public DisabledPeopleHousingType HousingType { get; set; }
}
