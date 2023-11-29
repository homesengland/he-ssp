using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DisabledPeopleClientGroupBasicModel : HomeTypeBasicModel
{
    public DisabledPeopleClientGroupBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public DisabledPeopleClientGroupBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public DisabledPeopleClientGroupType DisabledPeopleClientGroup { get; set; }
}
