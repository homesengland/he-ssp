using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

public static class FrontDoorProjectEnumMapping
{
    public static IDictionary<SupportActivityType, int?> ActivityType => new Dictionary<SupportActivityType, int?>
    {
        { SupportActivityType.AcquiringLand, (int)he_pipeline_he_ActivitiesinThisProject.Acquiringland },
        { SupportActivityType.DevelopingHomes, (int)he_pipeline_he_ActivitiesinThisProject.Developinghomesincludinganyminorsiterelatedinfrastructure },
        { SupportActivityType.ProvidingInfrastructure, (int)he_pipeline_he_ActivitiesinThisProject.Providinginfrastructure },
        { SupportActivityType.ManufacturingHomesWithinFactory, (int)he_pipeline_he_ActivitiesinThisProject.Manufacturinghomeswithinafactory },
        { SupportActivityType.SellingLandToHomesEngland, (int)he_pipeline_he_ActivitiesinThisProject.SellinglandtoHomesEngland },
        { SupportActivityType.Other, (int)he_pipeline_he_ActivitiesinThisProject.Other },
    };

    public static IDictionary<SitePlanningStatus, int?> PlanningStatus => new Dictionary<SitePlanningStatus, int?>
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
