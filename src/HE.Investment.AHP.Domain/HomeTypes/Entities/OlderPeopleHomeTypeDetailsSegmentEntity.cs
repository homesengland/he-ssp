using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.OlderPeople)]
public class OlderPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    public OlderPeopleHousingType HousingType { get; private set; }

    public void ChangeHousingType(OlderPeopleHousingType housingType)
    {
        HousingType = housingType;
    }

    public bool IsCompleted()
    {
        return HousingType != OlderPeopleHousingType.Undefined;
    }
}
