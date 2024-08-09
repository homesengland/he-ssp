using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.Common;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Queries;
using MediatR;
using Microsoft.FeatureManagement;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;

public sealed class AllocationBasicInfoMapper : IAllocationBasicInfoMapper
{
    private readonly IMediator _mediator;

    private readonly IFeatureManager _featureManager;

    public AllocationBasicInfoMapper(IMediator mediator, IFeatureManager featureManager)
    {
        _mediator = mediator;
        _featureManager = featureManager;
    }

    public async Task<AllocationBasicInfo> Map(AllocationClaimsDto allocation, CancellationToken cancellationToken)
    {
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocation.ProgrammeId)), cancellationToken);
        var isInContractDisabled = await _featureManager.IsEnabledAsync(FeatureFlags.DisableAhpIsInContract);

        return new AllocationBasicInfo(
            AllocationId.From(allocation.Id),
            new AllocationName(allocation.Name),
            new AllocationReferenceNumber(allocation.ReferenceNumber),
            new LocalAuthority(new LocalAuthorityCode(allocation.LocalAuthority.code), allocation.LocalAuthority.name),
            programme,
            ApplicationTenureMapper.ToDomain(allocation.Tenure)!.Value,
            isInContractDisabled || allocation.IsInContract);
    }
}
