using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Queries;
using MediatR;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;

public sealed class AllocationBasicInfoMapper : IAllocationBasicInfoMapper
{
    private readonly IMediator _mediator;

    public AllocationBasicInfoMapper(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<AllocationBasicInfo> Map(AhpAllocationDto allocation, CancellationToken cancellationToken)
    {
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocation.ProgrammeId)), cancellationToken);

        return new AllocationBasicInfo(
            AllocationId.From(allocation.Id),
            allocation.Name,
            allocation.ReferenceNumber,
            allocation.LocalAuthority.name,
            programme,
            AllocationTenureMapper.ToDomain(allocation.Tenure).Value);
    }
}
