using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.DisabledAndVulnerablePeople)]
public class DisabledPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public DisabledPeopleHomeTypeDetailsSegmentEntity(
        DisabledPeopleHousingType housingType = DisabledPeopleHousingType.Undefined,
        DisabledPeopleClientGroupType clientGroupType = DisabledPeopleClientGroupType.Undefined)
    {
        HousingType = housingType;
        ClientGroupType = clientGroupType;
    }

    public bool IsModified => _modificationTracker.IsModified;

    public DisabledPeopleHousingType HousingType { get; private set; }

    public DisabledPeopleClientGroupType ClientGroupType { get; private set; }

    public void ChangeHousingType(DisabledPeopleHousingType housingType)
    {
        HousingType = _modificationTracker.Change(HousingType, housingType);
    }

    public void ChangeClientGroupType(DisabledPeopleClientGroupType clientGroupType)
    {
        ClientGroupType = _modificationTracker.Change(ClientGroupType, clientGroupType);
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
