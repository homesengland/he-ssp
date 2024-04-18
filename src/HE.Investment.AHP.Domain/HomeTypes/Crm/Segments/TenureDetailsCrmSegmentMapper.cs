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
    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.TenureDetails;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new TenureDetailsSegmentEntity(
            dto.marketValue.IsProvided() ? new MarketValue((int)dto.marketValue!.Value.ToWholeNumberRoundFloor()) : null,
            dto.marketRent.IsProvided() ? new MarketRentPerWeek(dto.marketRent!.Value) : null,
            dto.prospectiveRent.IsProvided() ? new RentPerWeek(dto.prospectiveRent!.Value) : null,
            dto.prospectiveRentAsPercentOfMarketRent.IsProvided() ? new ProspectiveRentPercentage(dto.prospectiveRentAsPercentOfMarketRent!.Value) : null,
            YesNoTypeMapper.Map(dto.targetRentOver80PercentOfMarketRent),
            YesNoTypeMapper.Map(dto.RtSOExemption),
            MoreInformation.FromCrm(dto.exemptionJustification),
            dto.initialSalePercent.IsProvided() ? new InitialSale(dto.initialSalePercent!.Value) : null,
            dto.expectedFirstTrancheSaleReceipt.IsProvided() ? new ExpectedFirstTranche(dto.expectedFirstTrancheSaleReceipt!.Value) : null,
            dto.proposedRentAsPercentOfUnsoldShare.IsProvided() ? new ProspectiveRentPercentage(dto.proposedRentAsPercentOfUnsoldShare!.Value) : null);
    }

    protected override TenureDetailsSegmentEntity GetSegment(HomeTypeEntity entity) => entity.TenureDetails;

    protected override void MapToDto(HomeTypeDto dto, TenureDetailsSegmentEntity segment)
    {
        dto.marketValue = segment.MarketValue?.Value;
        dto.marketRent = segment.MarketRentPerWeek?.Value;
        dto.prospectiveRent = segment.RentPerWeek?.Value;
        dto.prospectiveRentAsPercentOfMarketRent = segment.ProspectiveRentAsPercentageOfMarketRent?.Value;
        dto.targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(segment.TargetRentExceedMarketRent?.Value);
        dto.RtSOExemption = YesNoTypeMapper.Map(segment.ExemptFromTheRightToSharedOwnership);
        dto.exemptionJustification = segment.ExemptionJustification?.Value;
        dto.initialSalePercent = segment.InitialSale?.Value;
        dto.expectedFirstTrancheSaleReceipt = segment.ExpectedFirstTranche?.Value;
        dto.proposedRentAsPercentOfUnsoldShare = segment.RentAsPercentageOfTheUnsoldShare?.Value;
    }
}
