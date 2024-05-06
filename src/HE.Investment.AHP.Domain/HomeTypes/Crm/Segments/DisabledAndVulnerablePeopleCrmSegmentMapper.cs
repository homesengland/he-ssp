using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class DisabledAndVulnerablePeopleCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<DisabledPeopleHomeTypeDetailsSegmentEntity>
{
    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.DisabledAndVulnerablePeople;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
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
            DisabledPeopleClientGroupType.PeopleAtRiskOfDomesticViolence => (int)invln_Clientgroup.WPeopleatriskofdomesticviolence,
            DisabledPeopleClientGroupType.PeopleWithAlcoholProblems => (int)invln_Clientgroup.APeoplewithalcoholproblems,
            DisabledPeopleClientGroupType.PeopleWithDrugProblems => (int)invln_Clientgroup.DPeoplewithdrugproblems,
            DisabledPeopleClientGroupType.PeopleWithHivOrAids => (int)invln_Clientgroup.XPeoplewithHIVorAIDS,
            DisabledPeopleClientGroupType.PeopleWithLearningDisabilitiesOrAutism => (int)invln_Clientgroup.NPeoplewithlearningdisabilitiesorautism,
            DisabledPeopleClientGroupType.PeopleWithMentalHealthProblems => (int)invln_Clientgroup.MPeoplewithmentalhealthproblems,
            DisabledPeopleClientGroupType.PeopleWithMultipleComplexNeeds => (int)invln_Clientgroup.GPeoplewithmultiplecomplexneeds,
            DisabledPeopleClientGroupType.PeopleWithPhysicalOrSensoryDisabilities => (int)invln_Clientgroup.PPeoplewithphysicalorsensorydisabilities,
            DisabledPeopleClientGroupType.MilitaryVeteransWithSupportNeeds => (int)invln_Clientgroup.BMilitaryveteranswithsupportneeds,
            DisabledPeopleClientGroupType.OffendersAndPeopleAtRiskOfOffending => (int)invln_Clientgroup.OOffendersandpeopleatriskofoffending,
            DisabledPeopleClientGroupType.HomelessFamiliesWithSupportNeeds => (int)invln_Clientgroup.QHomelessfamilieswithsupportneeds,
            DisabledPeopleClientGroupType.Refugees => (int)invln_Clientgroup.RRefugees,
            DisabledPeopleClientGroupType.RoughSleepers => (int)invln_Clientgroup.IRoughsleepers,
            DisabledPeopleClientGroupType.SingleHomelessPeopleWithSupportNeeds => (int)invln_Clientgroup.SSinglehomelesspeoplewithsupportneeds,
            DisabledPeopleClientGroupType.TeenageParents => (int)invln_Clientgroup.VTeenageparents,
            DisabledPeopleClientGroupType.YoungPeopleAtRisk => (int)invln_Clientgroup.YYoungpeopleatrisk,
            DisabledPeopleClientGroupType.YoungPeopleLeavingCare => (int)invln_Clientgroup.CYoungpeopleleavingcare,
            _ => null,
        };
    }

    private static DisabledPeopleClientGroupType MapClientGroup(int? value)
    {
        return value switch
        {
            (int)invln_Clientgroup.WPeopleatriskofdomesticviolence => DisabledPeopleClientGroupType.PeopleAtRiskOfDomesticViolence,
            (int)invln_Clientgroup.APeoplewithalcoholproblems => DisabledPeopleClientGroupType.PeopleWithAlcoholProblems,
            (int)invln_Clientgroup.DPeoplewithdrugproblems => DisabledPeopleClientGroupType.PeopleWithDrugProblems,
            (int)invln_Clientgroup.XPeoplewithHIVorAIDS => DisabledPeopleClientGroupType.PeopleWithHivOrAids,
            (int)invln_Clientgroup.NPeoplewithlearningdisabilitiesorautism => DisabledPeopleClientGroupType.PeopleWithLearningDisabilitiesOrAutism,
            (int)invln_Clientgroup.MPeoplewithmentalhealthproblems => DisabledPeopleClientGroupType.PeopleWithMentalHealthProblems,
            (int)invln_Clientgroup.GPeoplewithmultiplecomplexneeds => DisabledPeopleClientGroupType.PeopleWithMultipleComplexNeeds,
            (int)invln_Clientgroup.PPeoplewithphysicalorsensorydisabilities => DisabledPeopleClientGroupType.PeopleWithPhysicalOrSensoryDisabilities,
            (int)invln_Clientgroup.BMilitaryveteranswithsupportneeds => DisabledPeopleClientGroupType.MilitaryVeteransWithSupportNeeds,
            (int)invln_Clientgroup.OOffendersandpeopleatriskofoffending => DisabledPeopleClientGroupType.OffendersAndPeopleAtRiskOfOffending,
            (int)invln_Clientgroup.QHomelessfamilieswithsupportneeds => DisabledPeopleClientGroupType.HomelessFamiliesWithSupportNeeds,
            (int)invln_Clientgroup.RRefugees => DisabledPeopleClientGroupType.Refugees,
            (int)invln_Clientgroup.IRoughsleepers => DisabledPeopleClientGroupType.RoughSleepers,
            (int)invln_Clientgroup.SSinglehomelesspeoplewithsupportneeds => DisabledPeopleClientGroupType.SingleHomelessPeopleWithSupportNeeds,
            (int)invln_Clientgroup.VTeenageparents => DisabledPeopleClientGroupType.TeenageParents,
            (int)invln_Clientgroup.YYoungpeopleatrisk => DisabledPeopleClientGroupType.YoungPeopleAtRisk,
            (int)invln_Clientgroup.CYoungpeopleleavingcare => DisabledPeopleClientGroupType.YoungPeopleLeavingCare,
            _ => DisabledPeopleClientGroupType.Undefined,
        };
    }
}
