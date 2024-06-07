using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public sealed class PlanningStatusMapper : EnumMapper<SitePlanningStatus>
{
    protected override IDictionary<SitePlanningStatus, int?> Mapping => new Dictionary<SitePlanningStatus, int?>
    {
        { SitePlanningStatus.DetailedPlanningApprovalGranted, (int)invln_sitePlanningstatus._1DetailedPlanningApprovalgrantedwithnofurtherstepsrequiredbeforestartonsitecanoccur },
        { SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, (int)invln_sitePlanningstatus._2DetailedPlanningApprovalgrantedwithsomefurtherstepsrequiredbeforestartonsitecanoccur },
        { SitePlanningStatus.DetailedPlanningApplicationSubmitted, (int)invln_sitePlanningstatus._5DetailedPlanningSubmitted },
        { SitePlanningStatus.OutlinePlanningApprovalGranted, (int)invln_sitePlanningstatus._3OutlinePlanningApprovalgranted },
        { SitePlanningStatus.OutlinePlanningApplicationSubmitted, (int)invln_sitePlanningstatus._4OutlinePlanningSubmitted },
        { SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, (int)invln_sitePlanningstatus._6Planningdiscussionsunderwaywithplanningoffice },
        { SitePlanningStatus.NoProgressOnPlanningApplication, (int)invln_sitePlanningstatus._7Noprogressyetonplanningapplication },
        { SitePlanningStatus.NoPlanningRequired, (int)invln_sitePlanningstatus._8Noplanningrequired },
    };
}
