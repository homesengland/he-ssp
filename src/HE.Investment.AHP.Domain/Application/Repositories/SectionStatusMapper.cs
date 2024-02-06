using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public static class SectionStatusMapper
{
    private static readonly IDictionary<SectionStatus, invln_ahpsectioncompletionstatusset> SectionStatuses =
        new Dictionary<SectionStatus, invln_ahpsectioncompletionstatusset>
        {
            { SectionStatus.NotStarted, invln_ahpsectioncompletionstatusset.Notstarted },
            { SectionStatus.InProgress, invln_ahpsectioncompletionstatusset.InProgress },
            { SectionStatus.Completed, invln_ahpsectioncompletionstatusset.Completed },
        };

    public static SectionStatus ToDomain(int? value, ApplicationStatus? applicationStatus = null)
    {
        if (applicationStatus == ApplicationStatus.Withdrawn)
        {
            return SectionStatus.Withdrawn;
        }

        if (applicationStatus == ApplicationStatus.OnHold)
        {
            return SectionStatus.OnHold;
        }

        if (value == null)
        {
            return SectionStatus.NotStarted;
        }

        var contract = (invln_ahpsectioncompletionstatusset)value;
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
