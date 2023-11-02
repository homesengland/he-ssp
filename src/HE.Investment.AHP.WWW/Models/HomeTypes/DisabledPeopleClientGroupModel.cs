using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DisabledPeopleClientGroupModel : ProvidedHomeTypeModelBase
{
    public DisabledPeopleClientGroupModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public DisabledPeopleClientGroupModel()
        : this(string.Empty, string.Empty)
    {
    }

    public DisabledPeopleClientGroupType DisabledPeopleClientGroup { get; set; }
}
