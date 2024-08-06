using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
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

    public async Task<AllocationBasicInfo> Map(AllocationClaimsDto allocation, CancellationToken cancellationToken)
    {
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocation.ProgrammeId)), cancellationToken);

        return new AllocationBasicInfo(
            AllocationId.From(allocation.Id),
            new AllocationName(allocation.Name),
            new AllocationReferenceNumber(allocation.ReferenceNumber),
            new LocalAuthority(new LocalAuthorityCode(allocation.LocalAuthority.code), allocation.LocalAuthority.name),
            programme,
            ApplicationTenureMapper.ToDomain(allocation.Tenure)!.Value,
            true); // TODO: AB#104589 map from DTO when provided by CRM
    }
}
