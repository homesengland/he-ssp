using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.OlderPeople)]
public class OlderPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    public OlderPeopleHomeTypeDetailsSegmentEntity(OlderPeopleHousingType housingType = OlderPeopleHousingType.Undefined)
    {
        HousingType = housingType;
    }

    public OlderPeopleHousingType HousingType { get; private set; }

    public void ChangeHousingType(OlderPeopleHousingType housingType)
    {
        HousingType = housingType;
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new OlderPeopleHomeTypeDetailsSegmentEntity(HousingType);
    }

    public bool IsCompleted()
    {
        return HousingType != OlderPeopleHousingType.Undefined;
    }
}
