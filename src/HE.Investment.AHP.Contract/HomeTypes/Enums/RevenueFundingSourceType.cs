using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum RevenueFundingSourceType
{
    Undefined = 0,

    [Description("Charity")]
    Charity,

    [Description("Clinical Commissioning Group/Local Area Team")]
    ClinicalCommissioningGroupLocalAreaTeam,

    [Description("Crime and Disorder Reduction Partnerships")]
    CrimeAndDisorderReductionPartnerships,

    [Description("Department for Education")]
    DepartmentForEducation,

    [Description("Drugs Action Team")]
    DrugsActionTeam,

    [Description("Health and Well Being Board")]
    HealthAndWellBeingBoard,

    [Description("Home Office")]
    HomeOffice,

    [Description("Housing Department")]
    HousingDepartment,

    [Description("Local Area Agreements")]
    LocalAreaAgreements,

    [Description("National Lottery")]
    NationalLottery,

    [Description("NHS England")]
    NhsEngland,

    [Description("NHS Trust (e.g. Foundation Trust, Mental Health Trust)")]
    NhsTrust,

    [Description("Other health source")]
    OtherHealthSource,

    [Description("Other Local Authority Source")]
    OtherLocalAuthoritySource,

    [Description("Probation Service")]
    ProbationService,

    [Description("Provider's reserves")]
    ProvidersReserves,

    [Description("Social Services Department")]
    SocialServicesDepartment,

    [Description("Supporting People")]
    SupportingPeople,

    [Description("Youth Offending Teams")]
    YouthOffendingTeams,

    [Description("Other")]
    Other,
}
