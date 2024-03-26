using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.Common.CRM.Mappers;

internal class InternalPlanningStatusMapper : EnumMapper<SitePlanningStatus>, IPlanningStatusMapper
{
    protected override IDictionary<SitePlanningStatus, int?> Mapping => CommonEnumCrmMappings.PlanningStatus;
}
