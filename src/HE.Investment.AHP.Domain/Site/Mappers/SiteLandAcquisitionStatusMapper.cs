using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteLandAcquisitionStatusMapper : EnumMapper<SiteLandAcquisitionStatus>
{
    protected override IDictionary<SiteLandAcquisitionStatus, int?> Mapping =>
        new Dictionary<SiteLandAcquisitionStatus, int?>
        {
            { SiteLandAcquisitionStatus.FullOwnership, (int)invln_Landstatus._1Unconditionalacquisitionoffreeholdorlongleaseholdinteresthasoccurred },
            { SiteLandAcquisitionStatus.LandGifted, (int)invln_Landstatus._2LandpropertybeinggiftedorprovidedatadiscountbytheLA },
            { SiteLandAcquisitionStatus.ConditionalAcquisition, (int)invln_Landstatus._3Conditionalacquisitionlandoptionorheadsofterms },
            { SiteLandAcquisitionStatus.PurchaseNegotiationInProgress, (int)invln_Landstatus._4Landpropertypurchasenegotiationsunderway },
            { SiteLandAcquisitionStatus.PurchaseNegotiationsNotStarted, (int)invln_Landstatus._5Landpropertyidentifiedbutpurchasenegotiationsnotyetstarted },
        };
}
