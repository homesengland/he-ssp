using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.OlderPeople)]
public class OlderPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private OlderPeopleHousingType _housingType;

    public OlderPeopleHomeTypeDetailsSegmentEntity(OlderPeopleHousingType housingType = OlderPeopleHousingType.Undefined)
    {
        _housingType = housingType;
    }

    public bool IsModified { get; private set; }

    public OlderPeopleHousingType HousingType
    {
        get => _housingType;
        private set
        {
            if (_housingType != value)
            {
                IsModified = true;
            }

            _housingType = value;
        }
    }

    public void ChangeHousingType(OlderPeopleHousingType housingType)
    {
        HousingType = housingType;
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new OlderPeopleHomeTypeDetailsSegmentEntity(HousingType);
    }

    public bool IsRequired(HousingType housingType)
    {
        return housingType == Contract.HomeTypes.Enums.HousingType.HomesForOlderPeople;
    }

    public bool IsCompleted()
    {
        return HousingType != OlderPeopleHousingType.Undefined;
    }
}
