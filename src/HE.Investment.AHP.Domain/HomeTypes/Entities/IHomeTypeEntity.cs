using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeEntity
{
    ApplicationBasicInfo Application { get; }

    HomeTypeId? Id { get; }

    HomeTypeName? Name { get; }

    HousingType HousingType { get; }

    HomeInformationSegmentEntity HomeInformation { get; }

    DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails { get; }

    OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails { get; }

    DesignPlansSegmentEntity DesignPlans { get; }

    void ChangeHousingType(HousingType newHousingType);
}
