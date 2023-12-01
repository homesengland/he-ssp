using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSegmentEntity
{
    bool IsModified { get; }

    IHomeTypeSegmentEntity Duplicate();

    bool IsRequired(HousingType housingType);

    bool IsCompleted();

    void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType);
}
