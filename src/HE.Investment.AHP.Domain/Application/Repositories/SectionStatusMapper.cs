using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public static class SectionStatusMapper
{
    private static readonly IDictionary<SectionStatus, invln_sectioncompletionstatus> SectionStatuses =
        new Dictionary<SectionStatus, invln_sectioncompletionstatus>
        {
            { SectionStatus.NotStarted, invln_sectioncompletionstatus.Notstarted },
            { SectionStatus.InProgress, invln_sectioncompletionstatus.Inprogress },
            { SectionStatus.Completed, invln_sectioncompletionstatus.Completed },
        };

    public static SectionStatus ToDomain(int? value)
    {
        if (value == null)
        {
            return SectionStatus.NotStarted;
        }

        var contract = (invln_sectioncompletionstatus)value;
        if (SectionStatuses.TryGetKeyByValue(contract, out var status))
        {
            throw new ArgumentException($"Not supported Section Status value {value}");
        }

        return status;
    }
}

