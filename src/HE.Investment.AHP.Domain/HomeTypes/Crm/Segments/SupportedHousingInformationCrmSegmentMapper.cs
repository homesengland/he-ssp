using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class SupportedHousingInformationCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<SupportedHousingInformationSegmentEntity>
{
    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.SupportedHousingInformation;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new SupportedHousingInformationSegmentEntity(
            YesNoTypeMapper.Map(dto.localComissioningBodies),
            YesNoTypeMapper.Map(dto.shortStayAccommodation),
            MapRevenueFunding(dto.revenueFunding),
            dto.fundingSources.Select(MapSources),
            MoreInformation.Create(dto.moveOnArrangements),
            MoreInformation.Create(dto.typologyLocationAndDesign),
            MoreInformation.Create(dto.exitPlan));
    }

    protected override SupportedHousingInformationSegmentEntity GetSegment(HomeTypeEntity entity) => entity.SupportedHousingInformation;

    protected override void MapToDto(HomeTypeDto dto, SupportedHousingInformationSegmentEntity segment)
    {
        dto.localComissioningBodies = YesNoTypeMapper.Map(segment.LocalCommissioningBodiesConsulted);
        dto.shortStayAccommodation = YesNoTypeMapper.Map(segment.ShortStayAccommodation);
        dto.revenueFunding = MapRevenueFunding(segment.RevenueFundingType);
        dto.fundingSources = segment.RevenueFundingSources.Select(MapSources).ToList();
        dto.moveOnArrangements = segment.MoveOnArrangements?.Value;
        dto.typologyLocationAndDesign = segment.TypologyLocationAndDesign?.Value;
        dto.exitPlan = segment.ExitPlan?.Value;
    }

    private static int? MapRevenueFunding(RevenueFundingType? value)
    {
        return value switch
        {
            RevenueFundingType.RevenueFundingNeededAndIdentified => (int)invln_Revenuefunding.Yesrevenuefundingisneededandhasbeenidentified,
            RevenueFundingType.RevenueFundingNeededButNotIdentified => (int)invln_Revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified,
            RevenueFundingType.RevenueFundingNotNeeded => (int)invln_Revenuefunding.Norevenuefundingisnotneeded,
            _ => null,
        };
    }

    private static RevenueFundingType MapRevenueFunding(int? value)
    {
        return value switch
        {
            (int)invln_Revenuefunding.Yesrevenuefundingisneededandhasbeenidentified => RevenueFundingType.RevenueFundingNeededAndIdentified,
            (int)invln_Revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified => RevenueFundingType.RevenueFundingNeededButNotIdentified,
            (int)invln_Revenuefunding.Norevenuefundingisnotneeded => RevenueFundingType.RevenueFundingNotNeeded,
            _ => RevenueFundingType.Undefined,
        };
    }

    private static int MapSources(RevenueFundingSourceType value)
    {
        return value switch
        {
            RevenueFundingSourceType.Charity => (int)invln_Revenuefundingsources.Charity,
            RevenueFundingSourceType.ClinicalCommissioningGroupLocalAreaTeam => (int)invln_Revenuefundingsources.ClinicalCommissioningGroupLocalAreaTeam,
            RevenueFundingSourceType.CrimeAndDisorderReductionPartnerships => (int)invln_Revenuefundingsources.CrimeandDisorderReductionPartnerships,
            RevenueFundingSourceType.DepartmentForEducation => (int)invln_Revenuefundingsources.DepartmentforEducation,
            RevenueFundingSourceType.DrugsActionTeam => (int)invln_Revenuefundingsources.DrugsActionTeam,
            RevenueFundingSourceType.HealthAndWellBeingBoard => (int)invln_Revenuefundingsources.HealthandWellbeingBoard,
            RevenueFundingSourceType.HomeOffice => (int)invln_Revenuefundingsources.HomeOffice,
            RevenueFundingSourceType.HousingDepartment => (int)invln_Revenuefundingsources.HousingDepartment,
            RevenueFundingSourceType.LocalAreaAgreements => (int)invln_Revenuefundingsources.LocalAreaAgreements,
            RevenueFundingSourceType.NationalLottery => (int)invln_Revenuefundingsources.NationalLottery,
            RevenueFundingSourceType.NhsEngland => (int)invln_Revenuefundingsources.NHSEngland,
            RevenueFundingSourceType.NhsTrust => (int)invln_Revenuefundingsources.NHSTrustegFoundationTrustMentalhealthTrust,
            RevenueFundingSourceType.OtherHealthSource => (int)invln_Revenuefundingsources.Otherhealthsource,
            RevenueFundingSourceType.OtherLocalAuthoritySource => (int)invln_Revenuefundingsources.OtherLocalAuthoritySource,
            RevenueFundingSourceType.ProbationService => (int)invln_Revenuefundingsources.ProbationService,
            RevenueFundingSourceType.ProvidersReserves => (int)invln_Revenuefundingsources.Providersreserves,
            RevenueFundingSourceType.SocialServicesDepartment => (int)invln_Revenuefundingsources.SocialServicesDepartment,
            RevenueFundingSourceType.SupportingPeople => (int)invln_Revenuefundingsources.SupportingPeople,
            RevenueFundingSourceType.YouthOffendingTeams => (int)invln_Revenuefundingsources.YouthOffendingTeams,
            RevenueFundingSourceType.Other => (int)invln_Revenuefundingsources.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static RevenueFundingSourceType MapSources(int value)
    {
        return value switch
        {
            (int)invln_Revenuefundingsources.Charity => RevenueFundingSourceType.Charity,
            (int)invln_Revenuefundingsources.ClinicalCommissioningGroupLocalAreaTeam => RevenueFundingSourceType.ClinicalCommissioningGroupLocalAreaTeam,
            (int)invln_Revenuefundingsources.CrimeandDisorderReductionPartnerships => RevenueFundingSourceType.CrimeAndDisorderReductionPartnerships,
            (int)invln_Revenuefundingsources.DepartmentforEducation => RevenueFundingSourceType.DepartmentForEducation,
            (int)invln_Revenuefundingsources.DrugsActionTeam => RevenueFundingSourceType.DrugsActionTeam,
            (int)invln_Revenuefundingsources.HealthandWellbeingBoard => RevenueFundingSourceType.HealthAndWellBeingBoard,
            (int)invln_Revenuefundingsources.HomeOffice => RevenueFundingSourceType.HomeOffice,
            (int)invln_Revenuefundingsources.HousingDepartment => RevenueFundingSourceType.HousingDepartment,
            (int)invln_Revenuefundingsources.LocalAreaAgreements => RevenueFundingSourceType.LocalAreaAgreements,
            (int)invln_Revenuefundingsources.NationalLottery => RevenueFundingSourceType.NationalLottery,
            (int)invln_Revenuefundingsources.NHSEngland => RevenueFundingSourceType.NhsEngland,
            (int)invln_Revenuefundingsources.NHSTrustegFoundationTrustMentalhealthTrust => RevenueFundingSourceType.NhsTrust,
            (int)invln_Revenuefundingsources.Otherhealthsource => RevenueFundingSourceType.OtherHealthSource,
            (int)invln_Revenuefundingsources.OtherLocalAuthoritySource => RevenueFundingSourceType.OtherLocalAuthoritySource,
            (int)invln_Revenuefundingsources.ProbationService => RevenueFundingSourceType.ProbationService,
            (int)invln_Revenuefundingsources.Providersreserves => RevenueFundingSourceType.ProvidersReserves,
            (int)invln_Revenuefundingsources.SocialServicesDepartment => RevenueFundingSourceType.SocialServicesDepartment,
            (int)invln_Revenuefundingsources.SupportingPeople => RevenueFundingSourceType.SupportingPeople,
            (int)invln_Revenuefundingsources.YouthOffendingTeams => RevenueFundingSourceType.YouthOffendingTeams,
            (int)invln_Revenuefundingsources.Other => RevenueFundingSourceType.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
