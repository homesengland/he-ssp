using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSegmentEntity
{
    event EntityModifiedEventHandler SegmentModified;

    bool IsModified { get; }

    IHomeTypeSegmentEntity Duplicate();

    bool IsRequired(HousingType housingType);

    bool IsCompleted(HousingType housingType, Tenure tenure);

    void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType);
}
