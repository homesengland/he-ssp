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
            nameof(invln_HomeType.invln_targetrentover80ofmarketrent),
            nameof(invln_HomeType.invln_rtsoexempt),
            nameof(invln_HomeType.invln_reasonsforrtsoexemption),
            nameof(invln_HomeType.invln_initialsale),
            nameof(invln_HomeType.invln_expectedfirsttranchesalereceipt),
            nameof(invln_HomeType.invln_proposedrentasaofunsoldshare),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.TenureDetails;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new TenureDetailsSegmentEntity(
            dto.marketValue.IsProvided() ? new MarketValue(dto.marketValue!.Value) : null,
            dto.marketRent.IsProvided() ? new MarketRent(dto.marketRent!.Value) : null,
            dto.prospectiveRent.IsProvided() ? new ProspectiveRent(dto.prospectiveRent!.Value) : null,
            dto.prospectiveRentAsPercentOfMarketRent.IsProvided() ? new Percentage(dto.prospectiveRentAsPercentOfMarketRent!.Value) : null,
            YesNoTypeMapper.Map(dto.targetRentOver80PercentOfMarketRent),
            YesNoTypeMapper.Map(dto.RtSOExemption),
            dto.exemptionJustification.IsProvided() ? new MoreInformation(dto.exemptionJustification) : null,
            dto.initialSalePercent.IsProvided() ? new InitialSale(dto.initialSalePercent!.Value) : null,
            dto.expectedFirstTrancheSaleReceipt.IsProvided() ? new ExpectedFirstTranche(dto.expectedFirstTrancheSaleReceipt!.Value) : null,
            dto.proposedRentAsPercentOfUnsoldShare.IsProvided() ? new Percentage(dto.proposedRentAsPercentOfUnsoldShare!.Value) : null);
    }

    protected override TenureDetailsSegmentEntity GetSegment(HomeTypeEntity entity) => entity.TenureDetails;

    protected override void MapToDto(HomeTypeDto dto, TenureDetailsSegmentEntity segment)
    {
        dto.marketValue = segment.MarketValue?.Value;
        dto.marketRent = segment.MarketRent?.Value;
        dto.prospectiveRent = segment.ProspectiveRent?.Value;
        dto.prospectiveRentAsPercentOfMarketRent = segment.ProspectiveRentAsPercentageOfMarketRent?.Value;
        dto.targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(segment.TargetRentExceedMarketRent?.Value);
        dto.RtSOExemption = YesNoTypeMapper.Map(segment.ExemptFromTheRightToSharedOwnership);
        dto.exemptionJustification = segment.ExemptionJustification?.Value;
        dto.initialSalePercent = segment.InitialSale?.Value;
        dto.expectedFirstTrancheSaleReceipt = segment.ExpectedFirstTranche?.Value;
        dto.proposedRentAsPercentOfUnsoldShare = segment.SharedOwnershipRentAsPercentageOfTheUnsoldShare?.Value;
    }
}
