using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeEntity
{
    ApplicationBasicInfo Application { get; }

    HomeTypeId Id { get; }

    HomeTypeName Name { get; }

    HousingType HousingType { get; }

    DateTime? CreatedOn { get; }

    SectionStatus Status { get; }

    HomeInformationSegmentEntity HomeInformation { get; }

    DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails { get; }

    OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails { get; }

    DesignPlansSegmentEntity DesignPlans { get; }

    SupportedHousingInformationSegmentEntity SupportedHousingInformation { get; }

    TenureDetailsEntity TenureDetails { get; }

    void CompleteHomeType(IsSectionCompleted isSectionCompleted);
}
