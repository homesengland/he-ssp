using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DisabledPeopleClientGroupModel : HomeTypeBasicModel
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
