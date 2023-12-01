using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.OlderPeople)]
public class OlderPeopleHomeTypeDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public OlderPeopleHomeTypeDetailsSegmentEntity(OlderPeopleHousingType housingType = OlderPeopleHousingType.Undefined)
    {
        HousingType = housingType;
    }

    public bool IsModified => _modificationTracker.IsModified;

    public OlderPeopleHousingType HousingType { get; private set; }

    public void ChangeHousingType(OlderPeopleHousingType housingType)
    {
        HousingType = _modificationTracker.Change(HousingType, housingType);
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

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (sourceHousingType is Contract.HomeTypes.Enums.HousingType.HomesForOlderPeople)
        {
            ChangeHousingType(OlderPeopleHousingType.Undefined);
        }
    }
}
