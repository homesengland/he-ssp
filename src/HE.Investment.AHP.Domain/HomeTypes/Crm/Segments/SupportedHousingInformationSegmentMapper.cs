using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class SupportedHousingInformationSegmentMapper : HomeTypeCrmSegmentMapperBase<SupportedHousingInformationEntity>
{
    public SupportedHousingInformationSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_localcommissioningbodiesconsulted),
            nameof(invln_HomeType.invln_homesusedforshortstay),
            nameof(invln_HomeType.invln_revenuefunding),
            nameof(invln_HomeType.invln_revenuefundingsources),
            nameof(invln_HomeType.invln_moveonarrangementsforshortstayhomes),
            nameof(invln_HomeType.invln_typologylocationanddesing),
            nameof(invln_HomeType.invln_supportedhousingexitplan),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.SupportedHousingInformation;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        return new SupportedHousingInformationEntity(
            YesNoTypeMapper.Map(dto.localComissioningBodies),
            YesNoTypeMapper.Map(dto.shortStayAccommodation),
            MapRevenueFunding(dto.revenueFunding),
            dto.fundingSources.Select(MapSources),
            dto.moveOnArrangements.IsProvided() ? new MoreInformation(dto.moveOnArrangements) : null,
            dto.typologyLocationAndDesign.IsProvided() ? new MoreInformation(dto.typologyLocationAndDesign) : null,
            dto.exitPlan.IsProvided() ? new MoreInformation(dto.exitPlan) : null);
    }

    protected override SupportedHousingInformationEntity GetSegment(HomeTypeEntity entity) => entity.SupportedHousingInformation;

    protected override void MapToDto(HomeTypeDto dto, SupportedHousingInformationEntity segment)
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
            RevenueFundingType.RevenueFundingNeededAndIdentified => (int)invln_revenuefunding.Yesrevenuefundingisneededandhasbeenidentified,
            RevenueFundingType.RevenueFundingNeededButNotIdentified => (int)invln_revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified,
            RevenueFundingType.RevenueFundingNotNeeded => (int)invln_revenuefunding.Norevenuefundingisnotneeded,
            _ => null,
        };
    }

    private static RevenueFundingType MapRevenueFunding(int? value)
    {
        return value switch
        {
            (int)invln_revenuefunding.Yesrevenuefundingisneededandhasbeenidentified => RevenueFundingType.RevenueFundingNeededAndIdentified,
            (int)invln_revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified => RevenueFundingType.RevenueFundingNeededButNotIdentified,
            (int)invln_revenuefunding.Norevenuefundingisnotneeded => RevenueFundingType.RevenueFundingNotNeeded,
            _ => RevenueFundingType.Undefined,
        };
    }

    private static int MapSources(RevenueFundingSourceType value)
    {
        return value switch
        {
            RevenueFundingSourceType.Charity => (int)invln_revenuefundingsources.Charity,
            RevenueFundingSourceType.ClinicalCommissioningGroupLocalAreaTeam => (int)invln_revenuefundingsources.ClinicalCommissioningGroupLocalAreaTeam,
            RevenueFundingSourceType.CrimeAndDisorderReductionPartnerships => (int)invln_revenuefundingsources.CrimeandDisorderReductionPartnerships,
            RevenueFundingSourceType.DepartmentForEducation => (int)invln_revenuefundingsources.DepartmentforEducation,
            RevenueFundingSourceType.DrugsActionTeam => (int)invln_revenuefundingsources.DrugsActionTeam,
            RevenueFundingSourceType.HealthAndWellBeingBoard => (int)invln_revenuefundingsources.HealthandWellbeingBoard,
            RevenueFundingSourceType.HomeOffice => (int)invln_revenuefundingsources.HomeOffice,
            RevenueFundingSourceType.HousingDepartment => (int)invln_revenuefundingsources.HousingDepartment,
            RevenueFundingSourceType.LocalAreaAgreements => (int)invln_revenuefundingsources.LocalAreaAgreements,
            RevenueFundingSourceType.NationalLottery => (int)invln_revenuefundingsources.NationalLottery,
            RevenueFundingSourceType.NhsEngland => (int)invln_revenuefundingsources.NHSEngland,
            RevenueFundingSourceType.NhsTrust => (int)invln_revenuefundingsources.NHSTrust_egFoundationTrustMentalhealthTrust,
            RevenueFundingSourceType.OtherHealthSource => (int)invln_revenuefundingsources.Otherhealthsource,
            RevenueFundingSourceType.OtherLocalAuthoritySource => (int)invln_revenuefundingsources.OtherLocalAuthoritySource,
            RevenueFundingSourceType.ProbationService => (int)invln_revenuefundingsources.ProbationService,
            RevenueFundingSourceType.ProvidersReserves => (int)invln_revenuefundingsources.Providersreserves,
            RevenueFundingSourceType.SocialServicesDepartment => (int)invln_revenuefundingsources.SocialServicesDepartment,
            RevenueFundingSourceType.SupportingPeople => (int)invln_revenuefundingsources.SupportingPeople,
            RevenueFundingSourceType.YouthOffendingTeams => (int)invln_revenuefundingsources.YouthOffendingTeams,
            RevenueFundingSourceType.Other => (int)invln_revenuefundingsources.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static RevenueFundingSourceType MapSources(int value)
    {
        return value switch
        {
            (int)invln_revenuefundingsources.Charity => RevenueFundingSourceType.Charity,
            (int)invln_revenuefundingsources.ClinicalCommissioningGroupLocalAreaTeam => RevenueFundingSourceType.ClinicalCommissioningGroupLocalAreaTeam,
            (int)invln_revenuefundingsources.CrimeandDisorderReductionPartnerships => RevenueFundingSourceType.CrimeAndDisorderReductionPartnerships,
            (int)invln_revenuefundingsources.DepartmentforEducation => RevenueFundingSourceType.DepartmentForEducation,
            (int)invln_revenuefundingsources.DrugsActionTeam => RevenueFundingSourceType.DrugsActionTeam,
            (int)invln_revenuefundingsources.HealthandWellbeingBoard => RevenueFundingSourceType.HealthAndWellBeingBoard,
            (int)invln_revenuefundingsources.HomeOffice => RevenueFundingSourceType.HomeOffice,
            (int)invln_revenuefundingsources.HousingDepartment => RevenueFundingSourceType.HousingDepartment,
            (int)invln_revenuefundingsources.LocalAreaAgreements => RevenueFundingSourceType.LocalAreaAgreements,
            (int)invln_revenuefundingsources.NationalLottery => RevenueFundingSourceType.NationalLottery,
            (int)invln_revenuefundingsources.NHSEngland => RevenueFundingSourceType.NhsEngland,
            (int)invln_revenuefundingsources.NHSTrust_egFoundationTrustMentalhealthTrust => RevenueFundingSourceType.NhsTrust,
            (int)invln_revenuefundingsources.Otherhealthsource => RevenueFundingSourceType.OtherHealthSource,
            (int)invln_revenuefundingsources.OtherLocalAuthoritySource => RevenueFundingSourceType.OtherLocalAuthoritySource,
            (int)invln_revenuefundingsources.ProbationService => RevenueFundingSourceType.ProbationService,
            (int)invln_revenuefundingsources.Providersreserves => RevenueFundingSourceType.ProvidersReserves,
            (int)invln_revenuefundingsources.SocialServicesDepartment => RevenueFundingSourceType.SocialServicesDepartment,
            (int)invln_revenuefundingsources.SupportingPeople => RevenueFundingSourceType.SupportingPeople,
            (int)invln_revenuefundingsources.YouthOffendingTeams => RevenueFundingSourceType.YouthOffendingTeams,
            (int)invln_revenuefundingsources.Other => RevenueFundingSourceType.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
