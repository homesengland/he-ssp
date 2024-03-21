using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.Common.CRM.Mappers;

public class PlanningStatusMapper : EnumMapper<SitePlanningStatus>
{
    protected override IDictionary<SitePlanningStatus, int?> Mapping => CommonEnumCrmMappings.PlanningStatus;
}
