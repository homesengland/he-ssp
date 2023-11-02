using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.DisabledAndVulnerablePeople)]
public class DisabledPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    public DisabledPeopleHousingType HousingType { get; private set; }

    public DisabledPeopleClientGroupType ClientGroupType { get; private set; }

    public void ChangeHousingType(DisabledPeopleHousingType housingType)
    {
        HousingType = housingType;
    }

    public void ChangeClientGroupType(DisabledPeopleClientGroupType clientGroupType)
    {
        ClientGroupType = clientGroupType;
    }

    public bool IsCompleted()
    {
        return HousingType != DisabledPeopleHousingType.Undefined
               && ClientGroupType != DisabledPeopleClientGroupType.Undefined;
    }
}
