using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public sealed class MilestoneTypeMapper : EnumMapper<MilestoneType>
{
    protected override IDictionary<MilestoneType, int?> Mapping =>
        new Dictionary<MilestoneType, int?>
        {
            { MilestoneType.Acquisition, (int)invln_Milestone.Acquisition },
            { MilestoneType.StartOnSite, (int)invln_Milestone.SoS },
            { MilestoneType.Completion, (int)invln_Milestone.PC },
        };
}
