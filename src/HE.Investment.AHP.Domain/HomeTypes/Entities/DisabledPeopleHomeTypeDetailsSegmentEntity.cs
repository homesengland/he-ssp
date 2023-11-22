using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.DisabledAndVulnerablePeople)]
public class DisabledPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private DisabledPeopleHousingType _housingType;

    private DisabledPeopleClientGroupType _clientGroupType;

    public DisabledPeopleHomeTypeDetailsSegmentEntity(
        DisabledPeopleHousingType housingType = DisabledPeopleHousingType.Undefined,
        DisabledPeopleClientGroupType clientGroupType = DisabledPeopleClientGroupType.Undefined)
    {
        HousingType = housingType;
        ClientGroupType = clientGroupType;
    }

    public bool IsModified { get; private set; }

    public DisabledPeopleHousingType HousingType
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

    public DisabledPeopleClientGroupType ClientGroupType
    {
        get => _clientGroupType;
        private set
        {
            if (_clientGroupType != value)
            {
                IsModified = true;
            }

            _clientGroupType = value;
        }
    }

    public void ChangeHousingType(DisabledPeopleHousingType housingType)
    {
        HousingType = housingType;
    }

    public void ChangeClientGroupType(DisabledPeopleClientGroupType clientGroupType)
    {
        ClientGroupType = clientGroupType;
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new DisabledPeopleHomeTypeDetailsSegmentEntity(HousingType, ClientGroupType);
    }

    public bool IsRequired(HousingType housingType)
    {
        return housingType == Contract.HomeTypes.Enums.HousingType.HomesForDisabledAndVulnerablePeople;
    }

    public bool IsCompleted()
    {
        return HousingType != DisabledPeopleHousingType.Undefined
               && ClientGroupType != DisabledPeopleClientGroupType.Undefined;
    }
}
