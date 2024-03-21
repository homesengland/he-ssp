using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.Common.CRM.Mappers;

public static class CommonEnumCrmMappings
{
    public static IDictionary<SitePlanningStatus, int?> PlanningStatus => new Dictionary<SitePlanningStatus, int?>
    {
        { SitePlanningStatus.DetailedPlanningApprovalGranted, (int)invln_sitePlanningstatus._1DetailedPlanningApprovalgrantedwithnofurtherstepsrequiredbeforestartonsitecanoccur },
        { SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, (int)invln_sitePlanningstatus._2DetailedPlanningApprovalgrantedwithsomefurtherstepsrequiredbeforestartonsitecanoccur },
        { SitePlanningStatus.DetailedPlanningApplicationSubmitted, (int)invln_sitePlanningstatus._3OutlinePlanningApprovalgranted },
        { SitePlanningStatus.OutlinePlanningApprovalGranted, (int)invln_sitePlanningstatus._4OutlinePlanningSubmitted },
        { SitePlanningStatus.OutlinePlanningApplicationSubmitted, (int)invln_sitePlanningstatus._5DetailedPlanningSubmitted },
        { SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, (int)invln_sitePlanningstatus._6Planningdiscussionsunderwaywithplanningoffice },
        { SitePlanningStatus.NoProgressOnPlanningApplication, (int)invln_sitePlanningstatus._7Noprogressyetonplanningapplication },
        { SitePlanningStatus.NoPlanningRequired, (int)invln_sitePlanningstatus._8Noplanningrequired },
    };
}
