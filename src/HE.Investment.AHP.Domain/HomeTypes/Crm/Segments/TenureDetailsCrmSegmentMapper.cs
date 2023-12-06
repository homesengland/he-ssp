using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class TenureDetailsCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<TenureDetailsEntity>
{
    public TenureDetailsCrmSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_marketvalue),
            nameof(invln_HomeType.invln_marketrent),
            nameof(invln_HomeType.invln_prospectiverent),
            nameof(invln_HomeType.invln_prospectiverentasofmarketrent),
            nameof(invln_HomeType.invln_rtsoexempt),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.TenureDetails;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new TenureDetailsEntity(
            dto.marketValue.IsProvided() ? new HomeMarketValue(dto.marketValue!.Value) : null,
            dto.marketRent.IsProvided() ? new HomeWeeklyRent(dto.marketRent!.Value) : null,
            dto.prospectiveRent.IsProvided() ? new AffordableWeeklyRent(dto.prospectiveRent!.Value) : null,
            dto.prospectiveRentAsPercentOfMarketRent.IsProvided() ? new AffordableRentAsPercentageOfMarketRent(dto.prospectiveRentAsPercentOfMarketRent!.Value) : null,
            YesNoTypeMapper.Map(dto.RtSOExemption));
    }

    protected override TenureDetailsEntity GetSegment(HomeTypeEntity entity) => entity.TenureDetails;

    protected override void MapToDto(HomeTypeDto dto, TenureDetailsEntity segment)
    {
        dto.marketValue = segment.HomeMarketValue?.Value;
        dto.marketRent = segment.HomeWeeklyRent?.Value;
        dto.prospectiveRent = segment.AffordableWeeklyRent?.Value;
        dto.prospectiveRentAsPercentOfMarketRent = segment.AffordableRentAsPercentageOfMarketRent?.Value;
        dto.RtSOExemption = YesNoTypeMapper.Map(segment.TargetRentExceedMarketRent?.Value);
    }
}
