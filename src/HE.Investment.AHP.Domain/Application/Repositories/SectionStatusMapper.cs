using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public static class SectionStatusMapper
{
    private static readonly Dictionary<SectionStatus, invln_AHPSectioncompletionstatusSet> SectionStatuses =
        new()
        {
            { SectionStatus.NotStarted, invln_AHPSectioncompletionstatusSet.Notstarted },
            { SectionStatus.InProgress, invln_AHPSectioncompletionstatusSet.InProgress },
            { SectionStatus.Completed, invln_AHPSectioncompletionstatusSet.Completed },
        };

    public static SectionStatus ToDomain(int? value, ApplicationStatus? applicationStatus = null)
    {
        var statusMapping = new Dictionary<ApplicationStatus, SectionStatus>
        {
            { ApplicationStatus.Withdrawn, SectionStatus.Withdrawn },
            { ApplicationStatus.OnHold, SectionStatus.OnHold },
            { ApplicationStatus.RequestedEditing, SectionStatus.RequestedEditing },
            { ApplicationStatus.ApplicationSubmitted, SectionStatus.Submitted },
            { ApplicationStatus.UnderReview, SectionStatus.Submitted },
        };

        if (applicationStatus.HasValue && statusMapping.TryGetValue(applicationStatus.Value, out var mappedStatus))
        {
            return mappedStatus;
        }

        if (value == null)
        {
            return SectionStatus.NotStarted;
        }

        var contract = (invln_AHPSectioncompletionstatusSet)value;
        if (!SectionStatuses.TryGetKeyByValue(contract, out var status))
        {
            throw new ArgumentException($"Not supported Section Status value {value}");
        }

        return status;
    }

    public static int ToDto(SectionStatus value)
    {
        if (!SectionStatuses.TryGetValue(value, out var status))
        {
            throw new ArgumentException($"Not supported Section Status value {value}");
        }

        return (int)status;
    }
}
