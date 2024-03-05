using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteProcurementMapper : EnumMapper<SiteProcurement>
{
    protected override IDictionary<SiteProcurement, int?> Mapping =>
        new Dictionary<SiteProcurement, int?>
        {
            { SiteProcurement.LargeScaleContractProcurementAsIndividualProvider, (int)invln_Procurementmechanisms.Largescalecontractprocurement_858110000 },
            { SiteProcurement.LargeScaleContractProcurementThroughConsortium, (int)invln_Procurementmechanisms.Largescalecontractprocurement_858110001 },
            { SiteProcurement.BulkPurchaseOfComponents, (int)invln_Procurementmechanisms.Bulkpurchaseofcomponents },
            { SiteProcurement.PartneringSupplyChain, (int)invln_Procurementmechanisms.Partneringsupplychain },
            { SiteProcurement.PartneringArrangementsWithContractor, (int)invln_Procurementmechanisms.Partneringarrangementswithcontractor },
            { SiteProcurement.Other, (int)invln_Procurementmechanisms.Other },
            { SiteProcurement.None, (int)invln_Procurementmechanisms.Noneoftheabove },
        };
}
