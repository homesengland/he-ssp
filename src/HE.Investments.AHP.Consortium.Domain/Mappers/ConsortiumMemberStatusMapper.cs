using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.Consortium.Domain.Mappers;

public static class ConsortiumMemberStatusMapper
{
    public static ConsortiumMemberStatus ToDomain(int? value)
    {
        return value switch
        {
            (int?)invln_ConsortiumMember_StatusCode.Inactive => ConsortiumMemberStatus.Inactive,
            (int?)invln_ConsortiumMember_StatusCode.Additionapproved => ConsortiumMemberStatus.Active,
            (int?)invln_ConsortiumMember_StatusCode.Additionongoingcontracting => ConsortiumMemberStatus.PendingAddition,
            (int?)invln_ConsortiumMember_StatusCode.Additionsubmitted => ConsortiumMemberStatus.PendingAddition,
            (int?)invln_ConsortiumMember_StatusCode.Additionunderreview => ConsortiumMemberStatus.PendingAddition,
            (int?)invln_ConsortiumMember_StatusCode.Removalapproved => ConsortiumMemberStatus.Inactive,
            (int?)invln_ConsortiumMember_StatusCode.Removalongoingcontracting => ConsortiumMemberStatus.PendingRemoval,
            (int?)invln_ConsortiumMember_StatusCode.Removalsubmitted => ConsortiumMemberStatus.PendingRemoval,
            (int?)invln_ConsortiumMember_StatusCode.Removalunderreview => ConsortiumMemberStatus.PendingRemoval,
            _ => ConsortiumMemberStatus.Undefined,
        };
    }
}
