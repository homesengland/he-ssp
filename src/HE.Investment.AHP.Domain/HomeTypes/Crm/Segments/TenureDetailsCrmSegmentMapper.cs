using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class TenureDetailsCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<TenureDetailsSegmentEntity>
{
    public TenureDetailsCrmSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_marketvalue),
            nameof(invln_HomeType.invln_marketrent),
            nameof(invln_HomeType.invln_prospectiverent),
            nameof(invln_HomeType.invln_prospectiverentasofmarketrent),
            //nameof(invln_HomeType.invln_targetrentover80ofmarketrent), TODO wating for crm
            nameof(invln_HomeType.invln_rtsoexempt),
            //nameof(invln_HomeType.invln_reasonsforrtsoexemption), TODO wating for crm
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.TenureDetails;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new TenureDetailsSegmentEntity(
            dto.marketValue.IsProvided() ? new HomeMarketValue(dto.marketValue!.Value) : null,
            dto.marketRent.IsProvided() ? new HomeWeeklyRent(dto.marketRent!.Value) : null,
            dto.prospectiveRent.IsProvided() ? new AffordableWeeklyRent(dto.prospectiveRent!.Value) : null,
            dto.prospectiveRentAsPercentOfMarketRent.IsProvided() ? new AffordableRentAsPercentageOfMarketRent(dto.prospectiveRentAsPercentOfMarketRent!.Value) : null,
            //YesNoTypeMapper.Map(dto.target), TODO wating for crm
            YesNoTypeMapper.Map(dto.RtSOExemption), // todo
            YesNoTypeMapper.Map(dto.RtSOExemption),
            null); // todo
    }

    protected override TenureDetailsSegmentEntity GetSegment(HomeTypeEntity entity) => entity.TenureDetails;

    protected override void MapToDto(HomeTypeDto dto, TenureDetailsSegmentEntity segment)
    {
        dto.marketValue = segment.HomeMarketValue?.Value;
        dto.marketRent = segment.HomeWeeklyRent?.Value;
        dto.prospectiveRent = segment.AffordableWeeklyRent?.Value;
        dto.prospectiveRentAsPercentOfMarketRent = segment.AffordableRentAsPercentageOfMarketRent?.Value;
        //dto.target = YesNoTypeMapper.Map(segment.TargetRentExceedMarketRent?.Value); TODO wating for crm
        dto.RtSOExemption = YesNoTypeMapper.Map(segment.ExemptFromTheRightToSharedOwnership);
        //dto.reasonsforrtsoexemption = segment.ExitPlan?.Value; TODO wating for crm
    }
}
