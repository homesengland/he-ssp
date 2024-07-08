using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.Common.Contract;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Queries;
using MediatR;
using AhpProgramme = HE.Investments.Programme.Contract.Programme;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;

public class AllocationRepository : IAllocationRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IMediator _mediator;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    public AllocationRepository(IAllocationCrmContext allocationCrmContext, IMediator mediator, IPhaseCrmMapper phaseCrmMapper)
    {
        _allocationCrmContext = allocationCrmContext;
        _mediator = mediator;
        _phaseCrmMapper = phaseCrmMapper;
    }

    public async Task<AllocationEntity> GetById(AllocationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        var allocation = await _allocationCrmContext.GetById(id.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);

        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocation.ProgrammeId)), cancellationToken);

        return CreateEntity(allocation, programme);
    }

    private AllocationEntity CreateEntity(AhpAllocationDto allocationDto, AhpProgramme programme)
    {
        return new AllocationEntity(
            AllocationId.From(allocationDto.Id),
            new AllocationName(allocationDto.Name),
            new AllocationReferenceNumber(allocationDto.ReferenceNumber),
            new LocalAuthority(allocationDto.LocalAuthority.code, allocationDto.LocalAuthority.name),
            programme,
            AllocationTenureMapper.ToDomain(allocationDto.Tenure),
            new GrantDetails(allocationDto.GrantDetails.TotalGrantAllocated, allocationDto.GrantDetails.AmountPaid, allocationDto.GrantDetails.AmountRemaining),
            allocationDto.ListOfPhaseClaims.Select(x => _phaseCrmMapper.MapToDomain(x)).ToList());
    }
}
