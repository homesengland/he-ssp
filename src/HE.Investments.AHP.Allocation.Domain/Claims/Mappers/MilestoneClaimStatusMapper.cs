using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public sealed class MilestoneClaimStatusMapper : IMilestoneClaimStatusMapper
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public MilestoneClaimStatusMapper(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public MilestoneStatus MapStatus(Enums.MilestoneStatus status, DateTime forecastClaimDate)
    {
        return status switch
        {
            Enums.MilestoneStatus.Undefined => CalculateDueStatus(forecastClaimDate),
            Enums.MilestoneStatus.Draft => CalculateDueStatus(forecastClaimDate),
            Enums.MilestoneStatus.Submitted => MilestoneStatus.Submitted,
            Enums.MilestoneStatus.UnderReview => MilestoneStatus.UnderReview,
            Enums.MilestoneStatus.Approved => MilestoneStatus.Approved,
            Enums.MilestoneStatus.Rejected => MilestoneStatus.Rejected,
            Enums.MilestoneStatus.Reclaimed => MilestoneStatus.Reclaimed,
            _ => MilestoneStatus.Undefined,
        };
    }

    private MilestoneStatus CalculateDueStatus(DateTime forecastClaimDate)
    {
        var today = _dateTimeProvider.Now.Date;
        if (forecastClaimDate.Date.IsAfter(today.AddDays(14)))
        {
            return MilestoneStatus.Undefined;
        }

        if (forecastClaimDate.Date.IsAfter(today.AddDays(6)))
        {
            return MilestoneStatus.DueSoon;
        }

        if (forecastClaimDate.Date.IsAfter(today.AddDays(-7)))
        {
            return MilestoneStatus.Due;
        }

        return MilestoneStatus.Overdue;
    }
}
