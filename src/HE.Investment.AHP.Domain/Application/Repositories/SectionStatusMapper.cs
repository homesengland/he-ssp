using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Domain;
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

    public static SectionStatus ToDomain(int? value)
    {
        if (value == null)
        {
            return SectionStatus.NotStarted;
        }

        var contract = (invln_ahpsectioncompletionstatusset)value;
        if (SectionStatuses.TryGetKeyByValue(contract, out var status))
        {
            throw new ArgumentException($"Not supported Section Status value {value}");
        }

        return status;
    }
}
