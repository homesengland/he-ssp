using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

internal sealed class PlanningStatusMapper : EnumMapper<SitePlanningStatus>
{
    protected override IDictionary<SitePlanningStatus, int?> Mapping => new Dictionary<SitePlanningStatus, int?>
    {
        { SitePlanningStatus.DetailedPlanningApprovalGranted, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Detailedplanningapprovalgranted },
        { SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Detailedplanningapprovalgrantedwithsomefurtherstepsrequiredbeforestartonsitecanoccur },
        { SitePlanningStatus.DetailedPlanningApplicationSubmitted, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Detailedplanningapplicationsubmitted },
        { SitePlanningStatus.OutlinePlanningApprovalGranted, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Outlineplanningapprovalgranted },
        { SitePlanningStatus.OutlinePlanningApplicationSubmitted, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Outlineplanningapplicationsubmitted },
        { SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Planningdiscussionsunderwayswiththeplanningoffice },
        { SitePlanningStatus.NoProgressOnPlanningApplication, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Noprogressonplanningapplication },
        { SitePlanningStatus.NoPlanningRequired, (int)he_ProjectLocalAuthority_he_planningstatusofthesite.Noplanningrequired },
    };
}
