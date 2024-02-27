using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Data;

public static class SiteCrmFields
{
    public static readonly IReadOnlyList<string> Fields = new List<string>
    {
        nameof(invln_Sites.invln_SitesId),
        nameof(invln_Sites.invln_sitename),
        nameof(invln_Sites.invln_s106agreementinplace),
        nameof(invln_Sites.invln_developercontributionsforah),
        nameof(invln_Sites.invln_siteis100affordable),
        nameof(invln_Sites.invln_homesintheapplicationareadditional),
        nameof(invln_Sites.invln_anyrestrictionsinthes106),
        nameof(invln_Sites.invln_localauthorityconfirmationofadditionality),
        nameof(invln_Sites.invln_LocalAuthority),
        nameof(invln_Sites.invln_planningstatus),
        nameof(invln_Sites.invln_planningreferencenumber),
        nameof(invln_Sites.invln_detailedplanningapprovaldate),
        nameof(invln_Sites.invln_furtherstepsrequired),
        nameof(invln_Sites.invln_applicationfordetailedplanningsubmitted),
        nameof(invln_Sites.invln_expectedplanningapproval),
        nameof(invln_Sites.invln_outlineplanningapprovaldate),
        nameof(invln_Sites.invln_planningsubmissiondate),
        nameof(invln_Sites.invln_grantfundingforallhomes),
        nameof(invln_Sites.invln_landregistrytitle),
        nameof(invln_Sites.invln_landregistrytitlenumber),
        nameof(invln_Sites.invln_invlngrantfundingforallhomescoveredbytit),
        nameof(invln_Sites.invln_nationaldesignguideelements),
        nameof(invln_Sites.invln_assessedforbhl),
        nameof(invln_Sites.invln_bhlgreentrafficlights),
        nameof(invln_Sites.invln_landstatus),
        nameof(invln_Sites.invln_workstenderingstatus),
        nameof(invln_Sites.invln_maincontractorname),
        nameof(invln_Sites.invln_sme),
        nameof(invln_Sites.invln_intentiontoworkwithsme),
        nameof(invln_Sites.invln_strategicsite),
        nameof(invln_Sites.invln_StrategicSiteN),
        nameof(invln_Sites.invln_TypeofSite),
        nameof(invln_Sites.invln_greenbelt),
        nameof(invln_Sites.invln_regensite),
        nameof(invln_Sites.invln_streetfrontinfill),
        nameof(invln_Sites.invln_travellerpitchsite),
        nameof(invln_Sites.invln_travellerpitchsitetype),
        nameof(invln_Sites.invln_Ruralclassification),
        nameof(invln_Sites.invln_RuralExceptionSite),
        nameof(invln_Sites.invln_ActionstoReduce),
    };
}
