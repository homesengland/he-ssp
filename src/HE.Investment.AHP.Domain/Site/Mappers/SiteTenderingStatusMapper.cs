using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteTenderingStatusMapper : EnumMapper<SiteTenderingStatus>
{
    protected override IDictionary<SiteTenderingStatus, int?> Mapping =>
        new Dictionary<SiteTenderingStatus, int?>
        {
            { SiteTenderingStatus.UnconditionalWorksContract, (int)invln_Workstenderingstatus._1Unconditionalworkscontractletorworksbeingprovidedbyinhouseteam },
            { SiteTenderingStatus.ConditionalWorksContract, (int)invln_Workstenderingstatus._2Conditionalcontractletorpartneridentifiedbutnotyetincontract },
            { SiteTenderingStatus.TenderForWorksContract, (int)invln_Workstenderingstatus._3Tenderforworkscontractouttocompetition },
            { SiteTenderingStatus.ContractingHasNotYetBegun, (int)invln_Workstenderingstatus._4Workscontractingprocessnotyetbegun },
            { SiteTenderingStatus.NotApplicable, (int)invln_Workstenderingstatus._5NAWorksnotrequired },
        };
}
