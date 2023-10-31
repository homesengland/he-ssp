using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeEntity
{
    HomeTypeId? Id { get; }

    HomeTypeName? Name { get; }

    HousingType HousingType { get; }

    HomeInformationSegmentEntity HomeInformation { get; }

    DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails { get; }

    void ChangeHousingType(HousingType housingType);
}
