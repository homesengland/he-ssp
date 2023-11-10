using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum DisabledPeopleClientGroupType
{
    Undefined = 0,

    [Description("People at risk of domestic violence")]
    PeopleAtRiskOfDomesticViolence,

    [Description("People with alcohol problems")]
    PeopleWithAlcoholProblems,

    [Description("People with drug problems")]
    PeopleWithDrugProblems,

    [Description("People with HIV or AIDS")]
    PeopleWithHivOrAids,

    [Description("People with learning disabilities or autism")]
    PeopleWithLearningDisabilitiesOrAutism,

    [Description("People with mental health problems")]
    PeopleWithMentalHealthProblems,

    [Description("People with multiple complex needs")]
    PeopleWithMultipleComplexNeeds,

    [Description("People with physical or sensory disabilities")]
    PeopleWithPhysicalOrSensoryDisabilities,

    [Description("Military veterans with support needs")]
    MilitaryVeteransWithSupportNeeds,

    [Description("Offenders and people at risk of offending")]
    OffendersAndPeopleAtRiskOfOffending,

    [Description("Homeless families with support needs")]
    HomelessFamiliesWithSupportNeeds,

    [Description("Refugees")]
    Refugees,

    [Description("Rough sleepers")]
    RoughSleepers,

    [Description("Single homeless people with support needs")]
    SingleHomelessPeopleWithSupportNeeds,

    [Description("Teenage parents")]
    TeenageParents,

    [Description("Young people at risk")]
    YoungPeopleAtRisk,

    [Description("Young people leaving care")]
    YoungPeopleLeavingCare,
}
