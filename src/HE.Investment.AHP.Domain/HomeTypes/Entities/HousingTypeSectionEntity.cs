using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;

public class HousingTypeSectionEntity : IHomeTypeSectionEntity
{
    public HomeTypeSectionType SectionType => HomeTypeSectionType.HousingType;

    public HousingType HousingType { get; private set; }

    public bool IsCompleted()
    {
        return HousingType != HousingType.Undefined;
    }

    public void ChangeHousingType(HousingType housingType)
    {
        HousingType = housingType;
    }
}
