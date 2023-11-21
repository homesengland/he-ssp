using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class DisabledAndVulnerablePeopleCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<DisabledPeopleHomeTypeDetailsSegmentEntity>
{
    public DisabledAndVulnerablePeopleCrmSegmentMapper()
        : base(new[] { nameof(invln_HomeType.invln_typeofhousingfordisabledvulnerablepeople), nameof(invln_HomeType.invln_clientgroup), })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.DisabledAndVulnerablePeople;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        return new DisabledPeopleHomeTypeDetailsSegmentEntity(MapHousingType(dto.housingTypeForVulnerable), MapClientGroup(dto.clientGroup));
    }

    protected override DisabledPeopleHomeTypeDetailsSegmentEntity GetSegment(HomeTypeEntity entity) => entity.DisabledPeopleHomeTypeDetails;

    protected override void MapToDto(HomeTypeDto dto, DisabledPeopleHomeTypeDetailsSegmentEntity segment)
    {
        dto.housingTypeForVulnerable = MapHousingType(segment.HousingType);
        dto.clientGroup = MapClientGroup(segment.ClientGroupType);
    }

    private static int? MapHousingType(DisabledPeopleHousingType value)
    {
        return value switch
        {
            DisabledPeopleHousingType.DesignatedHomes => (int)invln_typeofhousingfordisabledvulnerable.Designatedhousingfordisabledandvulnerablepeoplewithaccesstosupport,
            DisabledPeopleHousingType.DesignatedSupportedHomes => (int)invln_typeofhousingfordisabledvulnerable.Designatedsupportedhousingfordisabledandvulnerablepeople,
            DisabledPeopleHousingType.PurposeDesignedHomes => (int)invln_typeofhousingfordisabledvulnerable.Purposedesignedhousingfordisabledandvulnerablepeoplewithaccesstosupport,
            DisabledPeopleHousingType.PurposeDesignedSupportedHomes => (int)invln_typeofhousingfordisabledvulnerable.Purposedesignedsupportedhousingfordisabledandvulnerablepeople,
            _ => null,
        };
    }

    private static DisabledPeopleHousingType MapHousingType(int? value)
    {
        return value switch
        {
            (int)invln_typeofhousingfordisabledvulnerable.Designatedhousingfordisabledandvulnerablepeoplewithaccesstosupport => DisabledPeopleHousingType.DesignatedHomes,
            (int)invln_typeofhousingfordisabledvulnerable.Designatedsupportedhousingfordisabledandvulnerablepeople => DisabledPeopleHousingType.DesignatedSupportedHomes,
            (int)invln_typeofhousingfordisabledvulnerable.Purposedesignedhousingfordisabledandvulnerablepeoplewithaccesstosupport => DisabledPeopleHousingType.PurposeDesignedHomes,
            (int)invln_typeofhousingfordisabledvulnerable.Purposedesignedsupportedhousingfordisabledandvulnerablepeople => DisabledPeopleHousingType.PurposeDesignedSupportedHomes,
            _ => DisabledPeopleHousingType.Undefined,
        };
    }

    private static int? MapClientGroup(DisabledPeopleClientGroupType value)
    {
        return value switch
        {
            DisabledPeopleClientGroupType.PeopleAtRiskOfDomesticViolence => (int)invln_clientgroup.WPeopleatriskofdomesticviolence,
            DisabledPeopleClientGroupType.PeopleWithAlcoholProblems => (int)invln_clientgroup.APeoplewithalcoholproblems,
            DisabledPeopleClientGroupType.PeopleWithDrugProblems => (int)invln_clientgroup.DPeoplewithdrugproblems,
            DisabledPeopleClientGroupType.PeopleWithHivOrAids => (int)invln_clientgroup.XPeoplewithHIVorAIDS,
            DisabledPeopleClientGroupType.PeopleWithLearningDisabilitiesOrAutism => (int)invln_clientgroup.NPeoplewithlearningdisabilitiesorautism,
            DisabledPeopleClientGroupType.PeopleWithMentalHealthProblems => (int)invln_clientgroup.MPeoplewithmentalhealthproblems,
            DisabledPeopleClientGroupType.PeopleWithMultipleComplexNeeds => (int)invln_clientgroup.GPeoplewithmultiplecomplexneeds,
            DisabledPeopleClientGroupType.PeopleWithPhysicalOrSensoryDisabilities => (int)invln_clientgroup.PPeoplewithphysicalorsensorydisabilities,
            DisabledPeopleClientGroupType.MilitaryVeteransWithSupportNeeds => (int)invln_clientgroup.BMilitaryveteranswithsupportneeds,
            DisabledPeopleClientGroupType.OffendersAndPeopleAtRiskOfOffending => (int)invln_clientgroup.OOffendersandpeopleatriskofoffending,
            DisabledPeopleClientGroupType.HomelessFamiliesWithSupportNeeds => (int)invln_clientgroup.QHomelessfamilieswithsupportneeds,
            DisabledPeopleClientGroupType.Refugees => (int)invln_clientgroup.RRefugees,
            DisabledPeopleClientGroupType.RoughSleepers => (int)invln_clientgroup.IRoughsleepers,
            DisabledPeopleClientGroupType.SingleHomelessPeopleWithSupportNeeds => (int)invln_clientgroup.SSinglehomelesspeoplewithsupportneeds,
            DisabledPeopleClientGroupType.TeenageParents => (int)invln_clientgroup.VTeenageparents,
            DisabledPeopleClientGroupType.YoungPeopleAtRisk => (int)invln_clientgroup.YYoungpeopleatrisk,
            DisabledPeopleClientGroupType.YoungPeopleLeavingCare => (int)invln_clientgroup.CYoungpeopleleavingcare,
            _ => null,
        };
    }

    private static DisabledPeopleClientGroupType MapClientGroup(int? value)
    {
        return value switch
        {
            (int)invln_clientgroup.WPeopleatriskofdomesticviolence => DisabledPeopleClientGroupType.PeopleAtRiskOfDomesticViolence,
            (int)invln_clientgroup.APeoplewithalcoholproblems => DisabledPeopleClientGroupType.PeopleWithAlcoholProblems,
            (int)invln_clientgroup.DPeoplewithdrugproblems => DisabledPeopleClientGroupType.PeopleWithDrugProblems,
            (int)invln_clientgroup.XPeoplewithHIVorAIDS => DisabledPeopleClientGroupType.PeopleWithHivOrAids,
            (int)invln_clientgroup.NPeoplewithlearningdisabilitiesorautism => DisabledPeopleClientGroupType.PeopleWithLearningDisabilitiesOrAutism,
            (int)invln_clientgroup.MPeoplewithmentalhealthproblems => DisabledPeopleClientGroupType.PeopleWithMentalHealthProblems,
            (int)invln_clientgroup.GPeoplewithmultiplecomplexneeds => DisabledPeopleClientGroupType.PeopleWithMultipleComplexNeeds,
            (int)invln_clientgroup.PPeoplewithphysicalorsensorydisabilities => DisabledPeopleClientGroupType.PeopleWithPhysicalOrSensoryDisabilities,
            (int)invln_clientgroup.BMilitaryveteranswithsupportneeds => DisabledPeopleClientGroupType.MilitaryVeteransWithSupportNeeds,
            (int)invln_clientgroup.OOffendersandpeopleatriskofoffending => DisabledPeopleClientGroupType.OffendersAndPeopleAtRiskOfOffending,
            (int)invln_clientgroup.QHomelessfamilieswithsupportneeds => DisabledPeopleClientGroupType.HomelessFamiliesWithSupportNeeds,
            (int)invln_clientgroup.RRefugees => DisabledPeopleClientGroupType.Refugees,
            (int)invln_clientgroup.IRoughsleepers => DisabledPeopleClientGroupType.RoughSleepers,
            (int)invln_clientgroup.SSinglehomelesspeoplewithsupportneeds => DisabledPeopleClientGroupType.SingleHomelessPeopleWithSupportNeeds,
            (int)invln_clientgroup.VTeenageparents => DisabledPeopleClientGroupType.TeenageParents,
            (int)invln_clientgroup.YYoungpeopleatrisk => DisabledPeopleClientGroupType.YoungPeopleAtRisk,
            (int)invln_clientgroup.CYoungpeopleleavingcare => DisabledPeopleClientGroupType.YoungPeopleLeavingCare,
            _ => DisabledPeopleClientGroupType.Undefined,
        };
    }
}
