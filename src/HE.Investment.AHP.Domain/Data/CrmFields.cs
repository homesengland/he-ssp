using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Data;

public static class CrmFields
{
    public static readonly IList<string> ApplicationToUpdate = new List<string> { nameof(invln_scheme.invln_schemename), nameof(invln_scheme.invln_Tenure), };

    public static readonly IList<string> ApplicationToRead = ApplicationToUpdate
        .Append(new List<string>
        {
            nameof(invln_scheme.invln_schemeinformationsectioncompletionstatus),
            nameof(invln_scheme.invln_hometypessectioncompletionstatus),
            nameof(invln_scheme.invln_financialdetailssectioncompletionstatus),
            nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus),
        });

    public static readonly IList<string> SchemeToUpdate = new List<string>
    {
        nameof(invln_scheme.invln_schemeinformationsectioncompletionstatus),
        nameof(invln_scheme.invln_fundingrequired),
        nameof(invln_scheme.invln_noofhomes),
        nameof(invln_scheme.invln_affordabilityevidence),
        nameof(invln_scheme.invln_sharedownershipsalesrisk),
        nameof(invln_scheme.invln_meetinglocalpriorities),
        nameof(invln_scheme.invln_meetinglocalhousingneed),
        nameof(invln_scheme.invln_discussionswithlocalstakeholders),
    };

    public static readonly IList<string> SchemeToRead = SchemeToUpdate
        .Append(new List<string> { nameof(invln_scheme.invln_schemename) });
}
