using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Data;

public static class CrmFields
{
    public static readonly IList<string> ApplicationToUpdate = new List<string>
    {
        nameof(invln_scheme.invln_schemename), nameof(invln_scheme.invln_Tenure), nameof(invln_scheme.statuscode),
    };

    public static readonly IList<string> ApplicationToRead = ApplicationToUpdate
        .Append(new List<string>
        {
            nameof(invln_scheme.invln_schemeinformationsectioncompletionstatus),
            nameof(invln_scheme.invln_hometypessectioncompletionstatus),
            nameof(invln_scheme.invln_financialdetailssectioncompletionstatus),
            nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus),
            nameof(invln_scheme.invln_lastexternalmodificationby),
            nameof(invln_scheme.invln_lastexternalmodificationon),
            nameof(invln_scheme.statuscode),
            nameof(invln_scheme.invln_pplicationid),
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
        .Append(new List<string> { nameof(invln_scheme.invln_schemename), nameof(invln_scheme.invln_Tenure), });

    public static readonly IList<string> FinancialDetailsToUpdate = new List<string>
    {
        nameof(invln_scheme.invln_actualacquisitioncost),
        nameof(invln_scheme.invln_expectedacquisitioncost),
        nameof(invln_scheme.invln_publicland),
        nameof(invln_scheme.invln_currentlandvalue),
        nameof(invln_scheme.invln_expectedoncosts),
        nameof(invln_scheme.invln_expectedonworks),
        nameof(invln_scheme.invln_borrowingagainstrentalincome),
        nameof(invln_scheme.invln_fundingfromopenmarkethomesonthisscheme),
        nameof(invln_scheme.invln_fundingfromopenmarkethomesnotonthisscheme),
        nameof(invln_scheme.invln_ownresources),
        nameof(invln_scheme.invln_recycledcapitalgrantfund),
        nameof(invln_scheme.invln_totalinitialsalesincome),
        nameof(invln_scheme.invln_othercapitalsources),
        nameof(invln_scheme.invln_transfervalue),
        nameof(invln_scheme.invln_grantsfromcountycouncil),
        nameof(invln_scheme.invln_grantsfromdhscextracare),
        nameof(invln_scheme.invln_grantsfromlocalauthority1),
        nameof(invln_scheme.invln_grantsfromsocialservices),
        nameof(invln_scheme.invln_grantsfromdhscnhsorotherhealth),
        nameof(invln_scheme.invln_grantsfromthelottery),
        nameof(invln_scheme.invln_grantsfromotherpublicbodies),
        nameof(invln_scheme.invln_financialdetailssectioncompletionstatus),
    };

    public static readonly IList<string> FinancialDetailsToRead = FinancialDetailsToUpdate
        .Append(new List<string> { nameof(invln_scheme.invln_schemename) });
}
