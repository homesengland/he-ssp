using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class BuildingForHealthyLifeTypeMapper : EnumMapper<BuildingForHealthyLifeType>
{
    protected override IDictionary<BuildingForHealthyLifeType, int?> Mapping =>
        new Dictionary<BuildingForHealthyLifeType, int?>
        {
            { BuildingForHealthyLifeType.Yes, (int)invln_AssessedforBHL.Yes },
            { BuildingForHealthyLifeType.No, (int)invln_AssessedforBHL.No },
            { BuildingForHealthyLifeType.NotApplicable, (int)invln_AssessedforBHL.NAmydevelopmentislessthan10homes },
            { BuildingForHealthyLifeType.Undefined, null },
        };

    protected override BuildingForHealthyLifeType? ToDomainMissing => BuildingForHealthyLifeType.Undefined;
}
